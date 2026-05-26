using System.Reflection;
using CampusInter.Domain.Common.Auditing.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Persistence.Extensions;

public static class ModelBuilderAuditExtensions
{
    public static void ApplyAuditColumnOrder(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Entidad
            var clrType = entityType.ClrType;

            if (clrType is null)
                continue;

            var entity = modelBuilder.Entity(clrType);

            // Propiedades de auditoría
            var auditNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(ICreatedEntity.CreatedAtUtc),
                nameof(ICreatedEntity.CreatedByUserId),
                nameof(IUpdatedEntity.LastModifiedAtUtc),
                nameof(IUpdatedEntity.LastModifiedByUserId),
                nameof(IDeletableEntity.IsDeleted),
                nameof(IDeletableEntity.DeletedAtUtc),
                nameof(IDeletableEntity.DeletedByUserId),
            };

            var order = 1;
            var primaryKey = entityType.FindPrimaryKey();

            // Llave primaria
            if (primaryKey is not null)
            {
                foreach (var property in primaryKey.Properties)
                {
                    entity.Property(property.Name).HasColumnOrder(order++);
                }
            }

            // Propiedades declaradas
            var declaredProperties = clrType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(property => entityType.FindProperty(property.Name) is not null)
                .ToList();

            // Orden de columnas propias
            foreach (var property in declaredProperties)
            {
                if (auditNames.Contains(property.Name))
                    continue;

                if (primaryKey?.Properties.Any(key => key.Name == property.Name) == true)
                    continue;

                entity.Property(property.Name).HasColumnOrder(order++);
            }

            // Auditoría de creación
            if (typeof(ICreatedEntity).IsAssignableFrom(clrType))
            {
                entity.Property(nameof(ICreatedEntity.CreatedAtUtc))
                    .IsRequired()
                    .HasColumnOrder(9000);

                entity.Property(nameof(ICreatedEntity.CreatedByUserId))
                    .HasMaxLength(100)
                    .HasColumnOrder(9001);
            }

            // Auditoría de modificación
            if (typeof(IUpdatedEntity).IsAssignableFrom(clrType))
            {
                entity.Property(nameof(IUpdatedEntity.LastModifiedAtUtc)).HasColumnOrder(9010);

                entity.Property(nameof(IUpdatedEntity.LastModifiedByUserId))
                    .HasMaxLength(100)
                    .HasColumnOrder(9011);
            }

            // Soft delete
            if (typeof(IDeletableEntity).IsAssignableFrom(clrType))
            {
                entity.Property(nameof(IDeletableEntity.IsDeleted))
                    .IsRequired()
                    .HasColumnOrder(9020);

                entity.Property(nameof(IDeletableEntity.DeletedAtUtc)).HasColumnOrder(9021);

                entity.Property(nameof(IDeletableEntity.DeletedByUserId))
                    .HasMaxLength(100)
                    .HasColumnOrder(9022);
            }
        }
    }
}
