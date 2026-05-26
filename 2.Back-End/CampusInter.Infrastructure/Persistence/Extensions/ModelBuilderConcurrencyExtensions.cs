using CampusInter.Domain.Common.Auditing.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CampusInter.Infrastructure.Persistence.Extensions;

public static class ModelBuilderConcurrencyExtensions
{
    public static ModelBuilder ApplyConcurrencyTokens(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Entidades con control de concurrencia
            if (!typeof(IHasConcurrencyToken).IsAssignableFrom(entityType.ClrType))
                continue;

            // RowVersion
            modelBuilder.Entity(entityType.ClrType)
                .Property<byte[]>(nameof(IHasConcurrencyToken.RowVersion))
                .IsRowVersion()
                .IsConcurrencyToken();
        }

        return modelBuilder;
    }
}
