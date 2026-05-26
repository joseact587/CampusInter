using CampusInter.Application.Common.Exceptions;
using CampusInter.Application.DTOs.Auth;
using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Application.Interfaces.Security;
using CampusInter.Application.Interfaces.Services;
using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Services;

public sealed class AuthService : IAuthService
{
    // Atributos
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    // Constructores
    public AuthService(
        IEstudianteRepository estudianteRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
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

        if (await _estudianteRepository.ExistePorCorreoAsync(correoNormalizado))
            throw new ConflictException("Ya existe un estudiante registrado con este correo.", "student_email_already_exists");

        if (await _estudianteRepository.ExistePorDocumentoAsync(documentoNormalizado))
            throw new ConflictException("Ya existe un estudiante registrado con este documento.", "student_document_already_exists");

        var passwordHash = _passwordHasher.Hash(request.Password);

        var estudiante = new Estudiante(
            request.PrimerNombre,
            request.SegundoNombre,
            request.PrimerApellido,
            request.SegundoApellido,
            correoNormalizado,
            documentoNormalizado,
            passwordHash);

        await _estudianteRepository.AgregarAsync(estudiante);

        await _unitOfWork.SaveChangesAsync();

        return BuildAuthResponse(estudiante);
    }

    public async Task<AuthResponse> IniciarSesionAsync(LoginRequest request)
    {
        var correoNormalizado = request.Correo.Trim().ToLowerInvariant();

        var estudiante = await _estudianteRepository.ObtenerPorCorreoAsync(correoNormalizado);

        if (estudiante is null)
            throw new BusinessValidationException("Credenciales invalidas.", "invalid_credentials");

        if (!estudiante.EstaActivo())
            throw new BusinessValidationException("El estudiante se encuentra inactivo.", "student_inactive");

        var passwordValid = _passwordHasher.Verify(estudiante.PasswordHash, request.Password);

        if (!passwordValid)
            throw new BusinessValidationException("Credenciales invalidas.", "invalid_credentials");

        return BuildAuthResponse(estudiante);
    }

    // Respuesta
    private AuthResponse BuildAuthResponse(Estudiante estudiante)
    {
        var accessToken = _jwtTokenService.GenerateToken(estudiante);

        return new AuthResponse
        {
            AccessToken = accessToken,
            EstudianteId = estudiante.EstudianteId,
            Correo = estudiante.Correo,
            PrimerNombre = estudiante.PrimerNombre,
            SegundoNombre = estudiante.SegundoNombre,
            PrimerApellido = estudiante.PrimerApellido,
            SegundoApellido = estudiante.SegundoApellido,
            Estado = estudiante.Estado.ToString()
        };
    }
}
