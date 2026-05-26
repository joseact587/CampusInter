using AutoMapper;
using CampusInter.Application.DTOs.Materias;
using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Application.Interfaces.Services;

namespace CampusInter.Application.Services;

public sealed class MateriaService : IMateriaService
{
    // Atributos
    private readonly IMateriaRepository _materiaRepository;
    private readonly IMapper _mapper;

    // Constructores
    public MateriaService(
        IMateriaRepository materiaRepository,
        IMapper mapper)
    {
        _materiaRepository = materiaRepository;
        _mapper = mapper;
    }

    // Consultas
    public async Task<IReadOnlyList<MateriaResponse>> GetMateriasDisponiblesAsync()
    {
        var materias = await _materiaRepository.GetActivasConProfesorAsync();

        return _mapper.Map<IReadOnlyList<MateriaResponse>>(materias);
    }
}
