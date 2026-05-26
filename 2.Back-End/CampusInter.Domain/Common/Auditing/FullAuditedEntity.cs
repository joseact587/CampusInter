using CampusInter.Domain.Common.Auditing.Interfaces;

namespace CampusInter.Domain.Common.Auditing;

public abstract class FullAuditedEntity : CreateUpdateEntity, IDeletableEntity
{
    public bool IsDeleted { get; protected set; }

    public DateTime? DeletedAtUtc { get; protected set; }

    public string? DeletedByUserId { get; protected set; }

    public void SetDeletedAudit(DateTime nowUtc, string userId)
    {
        IsDeleted = true;
        DeletedAtUtc = nowUtc;
        DeletedByUserId = userId;
    }
}
