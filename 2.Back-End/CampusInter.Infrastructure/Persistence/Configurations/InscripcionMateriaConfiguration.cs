using CampusInter.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusInter.Infrastructure.Persistence.Configurations;

public class InscripcionMateriaConfiguration : IEntityTypeConfiguration<InscripcionMateria>
{
    public void Configure(EntityTypeBuilder<InscripcionMateria> builder)
    {
        // Tabla
        builder.ToTable("InscripcionMaterias");

        // Llave primaria compuesta
        builder.HasKey(inscripcionMateria => new
        {
            inscripcionMateria.InscripcionId,
            inscripcionMateria.MateriaId
        });

        // Relaciones
        builder.HasOne(inscripcionMateria => inscripcionMateria.Inscripcion)
            .WithMany(inscripcion => inscripcion.InscripcionesMateria)
            .HasForeignKey(inscripcionMateria => inscripcionMateria.InscripcionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(inscripcionMateria => inscripcionMateria.Materia)
            .WithMany(materia => materia.InscripcionesMateria)
            .HasForeignKey(inscripcionMateria => inscripcionMateria.MateriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
