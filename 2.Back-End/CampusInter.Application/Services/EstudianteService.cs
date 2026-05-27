using AutoMapper;
using CampusInter.Application.Common.Exceptions;
using CampusInter.Application.DTOs.Estudiantes;
using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Application.Interfaces.Security;
using CampusInter.Application.Interfaces.Services;

namespace CampusInter.Application.Services;

public sealed class EstudianteService : IEstudianteService
{
    // Atributos
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    // Constructores
    public EstudianteService(
        IEstudianteRepository estudianteRepository,
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IMapper mapper)
    {
        _estudianteRepository = estudianteRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    // Metodos de estudiantes
    public async Task<IReadOnlyList<EstudianteResumenResponse>> ObtenerEstudiantesAsync()
    {
        var estudiantes = await _estudianteRepository.ObtenerActivosAsync();

        return _mapper.Map<IReadOnlyList<EstudianteResumenResponse>>(estudiantes);
    }

    public async Task<MiPerfilResponse> ObtenerMiPerfilAsync()
    {
        var estudianteId = ObtenerEstudianteIdAutenticado();

        var estudiante = await _estudianteRepository.ObtenerPorIdSinSeguimientoAsync(estudianteId);

        if (estudiante is null)
            throw new NotFoundException("El estudiante autenticado no existe.", "student_not_found");

        return _mapper.Map<MiPerfilResponse>(estudiante);
    }

    public async Task<MiPerfilResponse> ActualizarMiPerfilAsync(ActualizarMiPerfilRequest request)
    {
        var estudianteId = ObtenerEstudianteIdAutenticado();

        var estudiante = await _estudianteRepository.ObtenerPorIdAsync(estudianteId);

        if (estudiante is null)
            throw new NotFoundException("El estudiante autenticado no existe.", "student_not_found");

        var correoNormalizado = request.Correo.Trim().ToLowerInvariant();
        var correoExisteEnOtroUsuario = await _usuarioRepository.ExistePorCorreoEnOtroUsuarioAsync(
            correoNormalizado,
            estudiante.UsuarioId);

        if (correoExisteEnOtroUsuario)
            throw new ConflictException("Ya existe un usuario registrado con este correo.", "user_email_already_exists");

        var documentoExisteEnOtroEstudiante = await _estudianteRepository.ExistePorDocumentoEnOtroEstudianteAsync(
            request.Documento,
            estudiante.EstudianteId);

        if (documentoExisteEnOtroEstudiante)
            throw new ConflictException("Ya existe un estudiante registrado con este documento.", "student_document_already_exists");

        estudiante.ActualizarDatosBasicos(
            request.PrimerNombre,
            request.SegundoNombre,
            request.PrimerApellido,
            request.SegundoApellido,
            request.Documento);

        estudiante.Usuario.CambiarCorreo(correoNormalizado);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MiPerfilResponse>(estudiante);
    }

    public async Task InhabilitarMiPerfilAsync()
    {
        var estudianteId = ObtenerEstudianteIdAutenticado();

        var estudiante = await _estudianteRepository.ObtenerPorIdAsync(estudianteId);

        if (estudiante is null)
            throw new NotFoundException("El estudiante autenticado no existe.", "student_not_found");

        estudiante.Desactivar();

        await _unitOfWork.SaveChangesAsync();
    }

    // Usuario autenticado
    private int ObtenerEstudianteIdAutenticado()
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();

        var estudianteIdClaim = _currentUser.EstudianteId;
        var esEstudianteIdValido = int.TryParse(estudianteIdClaim, out var estudianteId);

        if (!esEstudianteIdValido)
            throw new BusinessValidationException("El usuario autenticado no tiene un perfil de estudiante válido.", "invalid_authenticated_student");

        return estudianteId;
    }
}
