namespace CampusInter.Api.Options;

public sealed class CorsOptions
{
    // Seccion de appsettings
    public const string SectionName = "Cors";

    // Origenes permitidos
    public string[] AllowedOrigins { get; init; } = [];
}
