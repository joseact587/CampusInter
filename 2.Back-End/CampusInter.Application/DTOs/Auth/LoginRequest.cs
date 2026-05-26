namespace CampusInter.Application.DTOs.Auth;

public sealed class LoginRequest
{
    // Atributos
    public string Correo { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
