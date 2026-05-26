namespace CampusInter.Domain.Common.Auditing.Interfaces;

public interface IUpdatedEntity
{
    DateTime? LastModifiedAtUtc { get; }
    string? LastModifiedByUserId { get; }

    void SetModifiedAudit(DateTime nowUtc, string userId);
}
