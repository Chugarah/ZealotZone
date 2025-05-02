namespace Core.Interfaces.Data;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

    // This is a non-generic version of SaveChangesAsync
    // This is used when we don't care about the return value
    Task<IEnumerable<object>> SaveChangesAsync(CancellationToken cancellationToken = default);

    // This is a generic version of SaveChangesAsync
    // This is used when we care about the return value
    Task<IEnumerable<TEntity>> SaveChangesAsync<TEntity>(
        CancellationToken cancellationToken = default
    )
        where TEntity : class;
    string GetDebugView();
    IEnumerable<TEntity> GetTrackedEntities<TEntity>()
        where TEntity : class;
}