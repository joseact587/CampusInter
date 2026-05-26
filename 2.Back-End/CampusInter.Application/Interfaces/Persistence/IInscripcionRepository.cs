using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Interfaces.Persistence;

public interface IInscripcionRepository
{
    Task AgregarAsync(Inscripcion inscripcion);

    Task<bool> TieneInscripcionActivaAsync(int estudianteId);

    Task<Inscripcion?> ObtenerActivaPorEstudianteIdAsync(int estudianteId);

    Task<IReadOnlyList<Inscripcion>> ObtenerActivasPorMateriasIdsAsync(
        IReadOnlyCollection<int> materiasIds,
        int estudianteIdExcluir);
}
