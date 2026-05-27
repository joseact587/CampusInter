using CampusInter.Application.DTOs.Inscripciones;

namespace CampusInter.Application.Interfaces.Services;

public interface IInscripcionService
{
    Task<InscripcionResponse> CrearInscripcionAsync(CrearInscripcionRequest request);

    Task<InscripcionResponse> ObtenerMiInscripcionAsync();

    Task<IReadOnlyList<MateriaCompanerosResponse>> ObtenerCompanerosPorMiInscripcionAsync();

    Task CancelarMiInscripcionAsync();
}
