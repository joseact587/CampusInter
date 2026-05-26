using CampusInter.Application.DTOs.Estudiantes;

namespace CampusInter.Application.Interfaces.Services;

public interface IEstudianteService
{
    Task<IReadOnlyList<EstudianteResumenResponse>> ObtenerEstudiantesAsync();

    Task<MiPerfilResponse> ObtenerMiPerfilAsync();
}
