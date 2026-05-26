using CampusInter.Application.Interfaces.Services;
using CampusInter.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CampusInter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Mapeos
        services.AddAutoMapper(configuracion =>
            configuracion.AddMaps(typeof(DependencyInjection).Assembly));

        // Servicios de aplicacion
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMateriaService, MateriaService>();
        services.AddScoped<IInscripcionService, InscripcionService>();

        return services;
    }
}
