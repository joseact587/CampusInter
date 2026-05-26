using CampusInter.Application.DTOs.Materias;

namespace CampusInter.Application.Interfaces.Services;

public interface IMateriaService
{
    Task<IReadOnlyList<MateriaResponse>> GetMateriasDisponiblesAsync();
}
