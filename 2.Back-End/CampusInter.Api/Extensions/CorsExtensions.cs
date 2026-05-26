using CampusInter.Api.Options;

namespace CampusInter.Api.Extensions;

public static class CorsExtensions
{
    // Nombre de la politica CORS
    public const string PolicyName = "CampusInterCorsPolicy";

    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Options
        services.Configure<CorsOptions>(
            configuration.GetSection(CorsOptions.SectionName));

        // Configuracion
        var corsOptions = configuration
            .GetSection(CorsOptions.SectionName)
            .Get<CorsOptions>() ?? new CorsOptions();

        // Politica CORS
        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy
                    .WithOrigins(corsOptions.AllowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
