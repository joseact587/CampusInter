using CampusInter.Domain.Entidades;
using CampusInter.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Profesor> Profesores => Set<Profesor>();
    public DbSet<Materia> Materias => Set<Materia>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();
    public DbSet<InscripcionMateria> InscripcionesMateria => Set<InscripcionMateria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuraciones de entidades
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Concurrencia
        modelBuilder.ApplyConcurrencyTokens();

        // Auditoría
        modelBuilder.ApplyAuditColumnOrder();

        base.OnModelCreating(modelBuilder);
    }
}
