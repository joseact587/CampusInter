using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Interfaces.Persistence;

public interface IMateriaRepository
{
    Task<IReadOnlyList<Materia>> GetActivasConProfesorAsync();

    Task<IReadOnlyList<Materia>> GetByIdsAsync(IReadOnlyCollection<int> materiasIds);
}
