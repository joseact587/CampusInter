using AutoMapper;
using CampusInter.Application.Common.Exceptions;
using CampusInter.Application.DTOs.Inscripciones;
using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Application.Interfaces.Security;
using CampusInter.Application.Interfaces.Services;
using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Services;

public sealed class InscripcionService : IInscripcionService
{
    // Atributos
    private readonly ICurrentUser _currentUser;
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IMateriaRepository _materiaRepository;
    private readonly IInscripcionRepository _inscripcionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    // Constructores
    public InscripcionService(
        ICurrentUser currentUser,
        IEstudianteRepository estudianteRepository,
        IMateriaRepository materiaRepository,
        IInscripcionRepository inscripcionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _estudianteRepository = estudianteRepository;
        _materiaRepository = materiaRepository;
        _inscripcionRepository = inscripcionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Metodos de inscripcion
    public async Task<InscripcionResponse> CrearInscripcionAsync(CrearInscripcionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var estudianteId = ObtenerEstudianteIdAutenticado();

        var estudiante = await _estudianteRepository.ObtenerPorIdAsync(estudianteId);

        if (estudiante is null)
            throw new NotFoundException("El estudiante autenticado no existe.", "student_not_found");

        if (!estudiante.EstaActivo())
            throw new BusinessValidationException("El estudiante se encuentra inactivo.", "student_inactive");

        var tieneInscripcionActiva = await _inscripcionRepository.TieneInscripcionActivaAsync(estudianteId);

        if (tieneInscripcionActiva)
            throw new ConflictException("El estudiante ya tiene una inscripcion activa.", "active_enrollment_exists");

        if (request.MateriasIds is null || request.MateriasIds.Count == 0)
            throw new BusinessValidationException("Debe seleccionar las materias de la inscripcion.", "subjects_required");

        var materiasIdsUnicas = request.MateriasIds
            .Distinct()
            .ToList();

        var materiasEncontradas = await _materiaRepository.GetByIdsAsync(materiasIdsUnicas);

        if (materiasEncontradas.Count != materiasIdsUnicas.Count)
            throw new BusinessValidationException("Una o mas materias seleccionadas no existen.", "subjects_not_found");

        var materiasPorId = materiasEncontradas.ToDictionary(materia => materia.MateriaId);
        var materias = request.MateriasIds
            .Select(materiaId => materiasPorId[materiaId])
            .ToList();

        var inscripcion = new Inscripcion(estudianteId, materias);

        await _inscripcionRepository.AgregarAsync(inscripcion);

        await _unitOfWork.SaveChangesAsync();

        var inscripcionCreada = await _inscripcionRepository.ObtenerActivaPorEstudianteIdAsync(estudianteId);

        return _mapper.Map<InscripcionResponse>(inscripcionCreada ?? inscripcion);
    }

    public async Task<InscripcionResponse> ObtenerMiInscripcionAsync()
    {
        var estudianteId = ObtenerEstudianteIdAutenticado();

        var inscripcion = await _inscripcionRepository.ObtenerActivaPorEstudianteIdAsync(estudianteId);

        if (inscripcion is null)
            throw new NotFoundException("El estudiante no tiene una inscripcion activa.", "active_enrollment_not_found");

        return _mapper.Map<InscripcionResponse>(inscripcion);
    }

    // Usuario autenticado
    private int ObtenerEstudianteIdAutenticado()
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();

        var usuarioId = _currentUser.UsuarioId;
        var esEstudianteIdValido = int.TryParse(usuarioId, out var estudianteId);

        if (!esEstudianteIdValido)
            throw new BusinessValidationException("El usuario autenticado no tiene un identificador valido.", "invalid_authenticated_user");

        return estudianteId;
    }

}
