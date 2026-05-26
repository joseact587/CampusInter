using CampusInter.Domain.Entidades;

namespace CampusInter.Application.Interfaces.Persistence;

public interface IEstudianteRepository
{
    Task<Estudiante?> ObtenerPorIdAsync(int estudianteId);

    Task<Estudiante?> ObtenerPorCorreoAsync(string correo);

    Task<bool> ExistePorCorreoAsync(string correo);

    Task<bool> ExistePorDocumentoAsync(string documento);

    Task AgregarAsync(Estudiante estudiante);
}
