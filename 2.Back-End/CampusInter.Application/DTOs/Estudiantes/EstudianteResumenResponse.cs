namespace CampusInter.Application.DTOs.Estudiantes;

public sealed class EstudianteResumenResponse
{
    // Atributos
    public int EstudianteId { get; init; }
    public string PrimerNombre { get; init; } = string.Empty;
    public string? SegundoNombre { get; init; }
    public string PrimerApellido { get; init; } = string.Empty;
    public string? SegundoApellido { get; init; }
    public string Estado { get; init; } = string.Empty;
}
