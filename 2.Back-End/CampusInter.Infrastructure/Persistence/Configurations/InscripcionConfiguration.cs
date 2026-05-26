using CampusInter.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusInter.Infrastructure.Persistence.Configurations;

public class InscripcionConfiguration : IEntityTypeConfiguration<Inscripcion>
{
    public void Configure(EntityTypeBuilder<Inscripcion> builder)
    {
        // Tabla
        builder.ToTable("Inscripciones");

        // Llave primaria
        builder.HasKey(inscripcion => inscripcion.InscripcionId);

        // Propiedades
        builder.Property(inscripcion => inscripcion.EstudianteId)
            .IsRequired();

        builder.Property(inscripcion => inscripcion.FechaInscripcion)
            .IsRequired();

        builder.Property(inscripcion => inscripcion.TotalCreditos)
            .IsRequired();

        builder.Property(inscripcion => inscripcion.Estado)
            .HasConversion<int>()
            .IsRequired();

        // Relaciones
        builder.HasOne(inscripcion => inscripcion.Estudiante)
            .WithMany(estudiante => estudiante.Inscripciones)
            .HasForeignKey(inscripcion => inscripcion.EstudianteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(inscripcion => inscripcion.InscripcionesMateria)
            .WithOne(inscripcionMateria => inscripcionMateria.Inscripcion)
            .HasForeignKey(inscripcionMateria => inscripcionMateria.InscripcionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Navegación
        builder.Navigation(inscripcion => inscripcion.InscripcionesMateria)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
