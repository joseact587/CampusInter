using CampusInter.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusInter.Infrastructure.Persistence.Configurations;

public class MateriaConfiguration : IEntityTypeConfiguration<Materia>
{
    public void Configure(EntityTypeBuilder<Materia> builder)
    {
        // Tabla
        builder.ToTable("Materias");

        // Llave primaria
        builder.HasKey(materia => materia.MateriaId);

        // Propiedades
        builder.Property(materia => materia.Nombre)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(materia => materia.Creditos)
            .IsRequired();

        builder.Property(materia => materia.ProfesorId)
            .IsRequired();

        builder.Property(materia => materia.Estado)
            .HasConversion<int>()
            .IsRequired();

        // Relaciones
        builder.HasOne(materia => materia.Profesor)
            .WithMany(profesor => profesor.Materias)
            .HasForeignKey(materia => materia.ProfesorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(materia => materia.InscripcionesMateria)
            .WithOne(inscripcionMateria => inscripcionMateria.Materia)
            .HasForeignKey(inscripcionMateria => inscripcionMateria.MateriaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Navegación
        builder.Navigation(materia => materia.InscripcionesMateria)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
