using CampusInter.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusInter.Infrastructure.Persistence.Configurations;

public class ProfesorConfiguration : IEntityTypeConfiguration<Profesor>
{
    public void Configure(EntityTypeBuilder<Profesor> builder)
    {
        // Tabla
        builder.ToTable("Profesores");

        // Llave primaria
        builder.HasKey(profesor => profesor.ProfesorId);

        // Propiedades
        builder.Property(profesor => profesor.PrimerNombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(profesor => profesor.SegundoNombre)
            .HasMaxLength(100);

        builder.Property(profesor => profesor.PrimerApellido)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(profesor => profesor.SegundoApellido)
            .HasMaxLength(100);

        builder.Property(profesor => profesor.Estado)
            .HasConversion<int>()
            .IsRequired();

        // Relaciones
        builder.HasMany(profesor => profesor.Materias)
            .WithOne(materia => materia.Profesor)
            .HasForeignKey(materia => materia.ProfesorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Navegación
        builder.Navigation(profesor => profesor.Materias)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
