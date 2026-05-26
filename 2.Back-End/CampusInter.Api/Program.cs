using CampusInter.Api.Extensions;
using CampusInter.Api.Middlewares;
using CampusInter.Application;
using CampusInter.Application.Interfaces.Security;
using CampusInter.Infrastructure;
using CampusInter.Infrastructure.Persistence;
using CampusInter.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddOpenApiDocumentation();

// CORS
builder.Services.AddCorsConfiguration(builder.Configuration);

// JWT Options / JWT Auth preparado
builder.Services.AddJwtAuth(builder.Configuration);

// Capas
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Manejo global de errores
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Manejo global de errores
app.UseExceptionHandler();

// Migraciones y seed inicial
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    await db.Database.MigrateAsync();
    await ApplicationDbContextSeeder.SeedAsync(db, passwordHasher);
}

// Swagger / OpenAPI
app.UseOpenApiDocumentation();

// Seguridad HTTP
app.UseHttpsRedirection();

// CORS
app.UseCors(CorsExtensions.PolicyName);

// Autenticacion
app.UseAuthentication();

// Autorizacion
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.Run();
