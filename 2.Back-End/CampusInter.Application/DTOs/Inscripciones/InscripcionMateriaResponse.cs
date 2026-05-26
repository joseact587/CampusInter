namespace CampusInter.Application.DTOs.Inscripciones;

public sealed class InscripcionMateriaResponse
{
    // Atributos
    public int MateriaId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public int Creditos { get; init; }
    public int ProfesorId { get; init; }
    public string ProfesorNombre { get; init; } = string.Empty;
}
