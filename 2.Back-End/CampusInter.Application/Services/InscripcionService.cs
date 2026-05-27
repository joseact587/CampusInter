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

    public async Task<IReadOnlyList<MateriaCompanerosResponse>> ObtenerCompanerosPorMiInscripcionAsync()
    {
        var estudianteId = ObtenerEstudianteIdAutenticado();

        var miInscripcion = await _inscripcionRepository.ObtenerActivaPorEstudianteIdAsync(estudianteId);

        if (miInscripcion is null)
            throw new NotFoundException("El estudiante no tiene una inscripcion activa.", "active_enrollment_not_found");

        var materiasInscritas = miInscripcion.InscripcionesMateria
            .Select(detalle => detalle.Materia)
            .OrderBy(materia => materia.MateriaId)
            .ToList();

        var materiasIds = materiasInscritas
            .Select(materia => materia.MateriaId)
            .ToList();

        var inscripcionesCompaneros = await _inscripcionRepository
            .ObtenerActivasPorMateriasIdsAsync(materiasIds, estudianteId);

        return materiasInscritas
            .Select(materia =>
            {
                var companeros = inscripcionesCompaneros
                    .Where(inscripcion => inscripcion.InscripcionesMateria
                        .Any(detalle => detalle.MateriaId == materia.MateriaId))
                    .Select(inscripcion => inscripcion.Estudiante)
                    .DistinctBy(estudiante => estudiante.EstudianteId)
                    .Select(estudiante => _mapper.Map<CompaneroResponse>(estudiante))
                    .ToList();

                return new MateriaCompanerosResponse
                {
                    MateriaId = materia.MateriaId,
                    MateriaNombre = materia.Nombre,
                    Companeros = companeros
                };
            })
            .ToList();
    }

    public async Task CancelarMiInscripcionAsync()
    {
        var estudianteId = ObtenerEstudianteIdAutenticado();

        var inscripcion = await _inscripcionRepository.ObtenerActivaPorEstudianteIdAsync(estudianteId);

        if (inscripcion is null)
            throw new NotFoundException("El estudiante no tiene una inscripcion activa.", "active_enrollment_not_found");

        inscripcion.Cancelar();

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
            throw new BusinessValidationException("El usuario autenticado no tiene un perfil de estudiante valido.", "invalid_authenticated_student");

        return estudianteId;
    }

}
