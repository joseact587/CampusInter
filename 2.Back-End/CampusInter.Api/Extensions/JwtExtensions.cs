using CampusInter.Api.Options;

namespace CampusInter.Api.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Options
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        // La autenticacion JWT real se configurara despues
        // cuando se implemente el servicio de generacion de tokens.
        return services;
    }
}
