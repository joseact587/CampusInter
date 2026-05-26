using CampusInter.Application.Interfaces.Security;
using CampusInter.Domain.Common.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CampusInter.Infrastructure.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    // Dependencias
    private readonly ICurrentUser _currentUser;

    // Constructor
    public AuditSaveChangesInterceptor(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    // Guardado síncrono
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    // Guardado asíncrono
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    // Auditoría
    private void ApplyAudit(DbContext? context)
    {
        if (context is null)
            return;

        // Usuario actual
        var nowUtc = DateTime.UtcNow;
        var userId = string.IsNullOrWhiteSpace(_currentUser.UsuarioId)
            ? "system"
            : _currentUser.UsuarioId;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            // Creación
            if (entry.State == EntityState.Added && entry.Entity is CreatedEntity createdEntity)
            {
                createdEntity.SetCreatedAudit(nowUtc, userId);
                continue;
            }

            // Modificación
            if (entry.State == EntityState.Modified && entry.Entity is CreateUpdateEntity updatedEntity)
            {
                updatedEntity.SetModifiedAudit(nowUtc, userId);
            }

            // Eliminación lógica
            if (entry.State == EntityState.Deleted && entry.Entity is FullAuditedEntity deletedEntity)
            {
                entry.State = EntityState.Modified;
                deletedEntity.SetDeletedAudit(nowUtc, userId);
            }
        }
    }
}
