namespace CampusInter.Application.DTOs.Inscripciones;

public sealed class InscripcionResponse
{
    // Atributos
    public int InscripcionId { get; init; }
    public int EstudianteId { get; init; }
    public DateTime FechaInscripcion { get; init; }
    public int TotalCreditos { get; init; }
    public string Estado { get; init; } = string.Empty;
    public IReadOnlyCollection<InscripcionMateriaResponse> Materias { get; init; } = [];
}
