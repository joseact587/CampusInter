using CampusInter.Domain.Common.Auditing.Interfaces;

namespace CampusInter.Domain.Common.Auditing;

public abstract class CreateUpdateEntity : CreatedEntity, IUpdatedEntity
{
    public DateTime? LastModifiedAtUtc { get; protected set; }

    public string? LastModifiedByUserId { get; protected set; }

    public void SetModifiedAudit(DateTime nowUtc, string userId)
    {
        LastModifiedAtUtc = nowUtc;
        LastModifiedByUserId = userId;
    }
}
