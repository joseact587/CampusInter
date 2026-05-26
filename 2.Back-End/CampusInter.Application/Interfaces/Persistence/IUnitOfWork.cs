namespace CampusInter.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void SetOriginalRowVersion<TEntity>(TEntity entity, byte[] rowVersion)
        where TEntity : class;
}
