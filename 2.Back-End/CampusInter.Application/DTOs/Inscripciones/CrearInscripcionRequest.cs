namespace CampusInter.Application.DTOs.Inscripciones;

public sealed class CrearInscripcionRequest
{
    // Atributos
    public IReadOnlyCollection<int> MateriasIds { get; init; } = [];
}
