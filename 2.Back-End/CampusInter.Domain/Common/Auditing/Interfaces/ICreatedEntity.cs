namespace CampusInter.Domain.Common.Auditing.Interfaces;

public interface ICreatedEntity
{
    DateTime CreatedAtUtc { get; }
    string? CreatedByUserId { get; }

    void SetCreatedAudit(DateTime nowUtc, string userId);
}
