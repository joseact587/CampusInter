using System.ComponentModel.DataAnnotations;

namespace CampusInter.Application.DTOs.Auth;

public sealed class RegisterRequest
{
    // Atributos
    [Required(ErrorMessage = "El primer nombre es obligatorio.")]
    public string PrimerNombre { get; init; } = string.Empty;
    public string? SegundoNombre { get; init; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    public string PrimerApellido { get; init; } = string.Empty;
    public string? SegundoApellido { get; init; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
    public string Correo { get; init; } = string.Empty;

    [Required(ErrorMessage = "El documento es obligatorio.")]
    public string Documento { get; init; } = string.Empty;

    [Required(ErrorMessage = "La contrasena es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contrasena debe tener minimo 6 caracteres.")]
    public string Password { get; init; } = string.Empty;
}
