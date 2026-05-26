using CampusInter.Domain.Common.Auditing.Interfaces;

namespace CampusInter.Domain.Common.Auditing;

public abstract class CreatedEntity : ICreatedEntity
{
    public DateTime CreatedAtUtc { get; protected set; }

    public string? CreatedByUserId { get; protected set; }

    public void SetCreatedAudit(DateTime nowUtc, string userId)
    {
        if (CreatedAtUtc == default)
            CreatedAtUtc = nowUtc;

        if (string.IsNullOrWhiteSpace(CreatedByUserId))
            CreatedByUserId = userId;
    }
}
