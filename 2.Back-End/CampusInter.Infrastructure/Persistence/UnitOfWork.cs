using CampusInter.Application.Interfaces.Persistence;

namespace CampusInter.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    // Atributos
    private readonly ApplicationDbContext _context;

    // Constructores
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    // Persistencia
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    // Concurrencia
    public void SetOriginalRowVersion<TEntity>(TEntity entity, byte[] rowVersion)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(rowVersion);

        _context.Entry(entity)
            .Property("RowVersion")
            .OriginalValue = rowVersion;
    }
}
