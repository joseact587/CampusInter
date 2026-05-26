using System.Security.Claims;
using System.Text;
using CampusInter.Api.Authorization;
using CampusInter.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

        // Configuracion
        var jwtOptions = configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>() ?? new JwtOptions();

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

        // Autenticacion
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),

                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        // Autorizacion
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.Estudiante, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Estudiante");
            });

            options.AddPolicy(AuthorizationPolicies.Administrador, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Administrador");
            });
        });

        return services;
    }
}
