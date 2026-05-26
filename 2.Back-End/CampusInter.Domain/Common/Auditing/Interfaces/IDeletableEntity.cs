namespace CampusInter.Domain.Common.Auditing.Interfaces;

public interface IDeletableEntity
{
    bool IsDeleted { get; }
    DateTime? DeletedAtUtc { get; }
    string? DeletedByUserId { get; }

    void SetDeletedAudit(DateTime nowUtc, string userId);
}
