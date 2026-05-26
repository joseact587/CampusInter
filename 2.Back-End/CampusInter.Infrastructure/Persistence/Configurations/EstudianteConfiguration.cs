using CampusInter.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusInter.Infrastructure.Persistence.Configurations;

public class EstudianteConfiguration : IEntityTypeConfiguration<Estudiante>
{
    public void Configure(EntityTypeBuilder<Estudiante> builder)
    {
        // Tabla
        builder.ToTable("Estudiantes");

        // Llave primaria
        builder.HasKey(estudiante => estudiante.EstudianteId);

        // Propiedades
        builder.Property(estudiante => estudiante.PrimerNombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(estudiante => estudiante.SegundoNombre)
            .HasMaxLength(100);

        builder.Property(estudiante => estudiante.PrimerApellido)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(estudiante => estudiante.SegundoApellido)
            .HasMaxLength(100);

        builder.Property(estudiante => estudiante.Correo)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(estudiante => estudiante.Documento)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(estudiante => estudiante.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(estudiante => estudiante.Estado)
            .HasConversion<int>()
            .IsRequired();

        // Índices
        builder.HasIndex(estudiante => estudiante.Correo)
            .IsUnique();

        builder.HasIndex(estudiante => estudiante.Documento)
            .IsUnique();

        // Relaciones
        builder.HasMany(estudiante => estudiante.Inscripciones)
            .WithOne(inscripcion => inscripcion.Estudiante)
            .HasForeignKey(inscripcion => inscripcion.EstudianteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Navegación
        builder.Navigation(estudiante => estudiante.Inscripciones)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
