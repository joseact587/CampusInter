namespace CampusInter.Domain.Common.Auditing.Interfaces;

public interface IHasConcurrencyToken
{
    byte[] RowVersion { get; }
}
