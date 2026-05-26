namespace CampusInter.Application.DTOs.Materias;

public sealed class MateriaResponse
{
    // Atributos
    public int MateriaId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public int Creditos { get; init; }
    public int ProfesorId { get; init; }
    public string ProfesorNombre { get; init; } = string.Empty;
}
