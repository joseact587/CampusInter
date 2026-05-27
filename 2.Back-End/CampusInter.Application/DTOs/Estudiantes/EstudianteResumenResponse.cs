namespace CampusInter.Application.DTOs.Estudiantes;

public sealed class EstudianteResumenResponse
{
    // Atributos
    public int EstudianteId { get; init; }
    public string PrimerNombre { get; init; } = string.Empty;
    public string? SegundoNombre { get; init; }
    public string PrimerApellido { get; init; } = string.Empty;
    public string? SegundoApellido { get; init; }
    public string Correo { get; init; } = string.Empty;
    public string Documento { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
}
