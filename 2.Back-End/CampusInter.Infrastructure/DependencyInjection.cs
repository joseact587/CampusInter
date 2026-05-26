using CampusInter.Application.Interfaces.Persistence;
using CampusInter.Application.Interfaces.Security;
using CampusInter.Infrastructure.Persistence;
using CampusInter.Infrastructure.Persistence.Interceptors;
using CampusInter.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CampusInter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Usuario actual
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        // Interceptores
        services.AddScoped<AuditSaveChangesInterceptor>();

        // Persistencia
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var auditInterceptor = serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(auditInterceptor);
        });

        return services;
    }
}
