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
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    // Constructores
    public EstudianteService(
        IEstudianteRepository estudianteRepository,
        ICurrentUser currentUser,
        IMapper mapper)
    {
        _estudianteRepository = estudianteRepository;
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
