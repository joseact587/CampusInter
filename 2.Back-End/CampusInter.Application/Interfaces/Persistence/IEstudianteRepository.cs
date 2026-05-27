using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Interfaces.Persistence;

public interface IEstudianteRepository
{
    Task<IReadOnlyList<Estudiante>> ObtenerActivosAsync();

    Task<Estudiante?> ObtenerPorIdAsync(int estudianteId);

    Task<Estudiante?> ObtenerPorIdSinSeguimientoAsync(int estudianteId);

    Task<bool> ExistePorDocumentoAsync(string documento);

    Task<bool> ExistePorDocumentoEnOtroEstudianteAsync(string documento, int estudianteId);

    Task AgregarAsync(Estudiante estudiante);
}
