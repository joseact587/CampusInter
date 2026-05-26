namespace CampusInter.Application.DTOs.Auth;

public sealed class AuthResponse
{
    // Atributos
    public string AccessToken { get; init; } = string.Empty;
    public int UsuarioId { get; init; }
    public int EstudianteId { get; init; }
    public string Correo { get; init; } = string.Empty;
    public string PrimerNombre { get; init; } = string.Empty;
    public string? SegundoNombre { get; init; }
    public string PrimerApellido { get; init; } = string.Empty;
    public string? SegundoApellido { get; init; }
    public string Rol { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
}
