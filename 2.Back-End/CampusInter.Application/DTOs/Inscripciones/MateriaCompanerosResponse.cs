namespace CampusInter.Application.DTOs.Inscripciones;

public sealed class MateriaCompanerosResponse
{
    // Atributos
    public int MateriaId { get; init; }
    public string MateriaNombre { get; init; } = string.Empty;
    public IReadOnlyCollection<CompaneroResponse> Companeros { get; init; } = [];
}
