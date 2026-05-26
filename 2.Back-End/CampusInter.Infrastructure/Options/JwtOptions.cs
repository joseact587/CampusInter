namespace CampusInter.Infrastructure.Options;

public sealed class JwtOptions
{
    // Seccion de appsettings
    public const string SectionName = "Jwt";

    // Atributos
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; }
}
