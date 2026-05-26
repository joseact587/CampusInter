namespace CampusInter.Application.DTOs.Auth;

public sealed class RegisterRequest
{
    // Atributos
    public string PrimerNombre { get; init; } = string.Empty;
    public string? SegundoNombre { get; init; }
    public string PrimerApellido { get; init; } = string.Empty;
    public string? SegundoApellido { get; init; }
    public string Correo { get; init; } = string.Empty;
    public string Documento { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
