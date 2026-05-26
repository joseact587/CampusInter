using CampusInter.Application.Interfaces.Services;
using CampusInter.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CampusInter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Servicios de aplicacion
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
