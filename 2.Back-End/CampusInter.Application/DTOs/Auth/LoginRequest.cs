using System.ComponentModel.DataAnnotations;

namespace CampusInter.Application.DTOs.Auth;

public sealed class LoginRequest
{
    // Atributos
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
    public string Correo { get; init; } = string.Empty;

    [Required(ErrorMessage = "La contrasena es obligatoria.")]
    public string Password { get; init; } = string.Empty;
}
