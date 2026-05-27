using System.ComponentModel.DataAnnotations;

namespace CampusInter.Application.DTOs.Estudiantes;

public sealed class ActualizarMiPerfilRequest
{
    // Atributos
    [Required(ErrorMessage = "El primer nombre es obligatorio.")]
    public string PrimerNombre { get; init; } = string.Empty;

    public string? SegundoNombre { get; init; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    public string PrimerApellido { get; init; } = string.Empty;

    public string? SegundoApellido { get; init; }

    [Required(ErrorMessage = "El documento es obligatorio.")]
    public string Documento { get; init; } = string.Empty;
}
