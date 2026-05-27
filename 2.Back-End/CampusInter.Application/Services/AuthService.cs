using CampusInter.Application.Common.Exceptions;
using CampusInter.Application.DTOs.Auth;
using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Application.Interfaces.Security;
using CampusInter.Application.Interfaces.Services;
using CampusInter.Domain.Entidades;
using CampusInter.Domain.Enums;

namespace CampusInter.Application.Services;

public sealed class AuthService : IAuthService
{
    // Atributos
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    // Constructores
    public AuthService(
        IUsuarioRepository usuarioRepository,
        IEstudianteRepository estudianteRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _usuarioRepository = usuarioRepository;
        _estudianteRepository = estudianteRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    // Metodos de autenticacion
    public async Task<AuthResponse> RegistrarAsync(RegisterRequest request)
    {
        var correoNormalizado = request.Correo.Trim().ToLowerInvariant();
        var documentoNormalizado = request.Documento.Trim();

        if (await _usuarioRepository.ExistePorCorreoAsync(correoNormalizado))
            throw new ConflictException("Ya existe un usuario registrado con este correo.", "user_email_already_exists");

        if (await _estudianteRepository.ExistePorDocumentoAsync(documentoNormalizado))
            throw new ConflictException("Ya existe un estudiante registrado con este documento.", "student_document_already_exists");

        var passwordHash = _passwordHasher.Hash(request.Password);

        var usuario = new Usuario(
            correoNormalizado,
            passwordHash,
            RolUsuario.Estudiante);

        await _usuarioRepository.AgregarAsync(usuario);

        await _unitOfWork.SaveChangesAsync();

        var estudiante = new Estudiante(
            usuario.UsuarioId,
            request.PrimerNombre,
            request.SegundoNombre,
            request.PrimerApellido,
            request.SegundoApellido,
            documentoNormalizado);

        await _estudianteRepository.AgregarAsync(estudiante);

        await _unitOfWork.SaveChangesAsync();

        usuario = await _usuarioRepository.ObtenerPorIdAsync(usuario.UsuarioId)
            ?? throw new NotFoundException("El usuario registrado no fue encontrado.", "user_not_found");

        if (usuario.Estudiante is null)
            throw new NotFoundException("El perfil de estudiante no fue encontrado.", "student_profile_not_found");

        return ConstruirRespuestaAutenticacion(usuario, usuario.Estudiante);
    }

    public async Task<AuthResponse> IniciarSesionAsync(LoginRequest request)
    {
        var correoNormalizado = request.Correo.Trim().ToLowerInvariant();

        var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(correoNormalizado);

        if (usuario is null)
            throw new BusinessValidationException("Credenciales invalidas.", "invalid_credentials");

        if (!usuario.EstaActivo())
            throw new BusinessValidationException("El usuario se encuentra inactivo.", "user_inactive");

        if (usuario.Rol != RolUsuario.Estudiante)
            throw new BusinessValidationException("El usuario no tiene perfil de estudiante.", "invalid_user_role");

        if (usuario.Estudiante is null)
            throw new BusinessValidationException("El usuario no tiene un perfil de estudiante asociado.", "student_profile_not_found");

        //if (!usuario.Estudiante.EstaActivo())
        //    throw new BusinessValidationException("El estudiante se encuentra inactivo.", "student_inactive");

        var passwordValid = _passwordHasher.Verify(usuario.PasswordHash, request.Password);

        if (!passwordValid)
            throw new BusinessValidationException("Credenciales invalidas.", "invalid_credentials");

        return ConstruirRespuestaAutenticacion(usuario, usuario.Estudiante);
    }

    private AuthResponse ConstruirRespuestaAutenticacion(Usuario usuario, Estudiante estudiante)
    {
        var accessToken = _jwtTokenService.GenerateToken(usuario, estudiante);

        return new AuthResponse
        {
            AccessToken = accessToken,
            UsuarioId = usuario.UsuarioId,
            EstudianteId = estudiante.EstudianteId,
            Correo = usuario.Correo,
            PrimerNombre = estudiante.PrimerNombre,
            SegundoNombre = estudiante.SegundoNombre,
            PrimerApellido = estudiante.PrimerApellido,
            SegundoApellido = estudiante.SegundoApellido,
            Rol = usuario.Rol.ToString(),
            Roles = [usuario.Rol.ToString()],
            Estado = estudiante.Estado.ToString()
        };
    }
}
