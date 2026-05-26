using CampusInter.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusInter.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Tabla
        builder.ToTable("Usuarios");

        // Llave primaria
        builder.HasKey(usuario => usuario.UsuarioId);

        // Propiedades
        builder.Property(usuario => usuario.Correo)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(usuario => usuario.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(usuario => usuario.Rol)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(usuario => usuario.Estado)
            .HasConversion<int>()
            .IsRequired();

        // Indices
        builder.HasIndex(usuario => usuario.Correo)
            .IsUnique();

        // Filtros
        builder.HasQueryFilter(usuario => !usuario.IsDeleted);

        // Relaciones
        builder.HasOne(usuario => usuario.Estudiante)
            .WithOne(estudiante => estudiante.Usuario)
            .HasForeignKey<Estudiante>(estudiante => estudiante.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
