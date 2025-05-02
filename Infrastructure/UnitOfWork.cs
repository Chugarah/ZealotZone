
using Core.Interfaces.Data;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure;

/// <summary>
/// Using AI Phind to create Summary
/// Coordinates database operations and transactions across multiple repositories
/// </summary>
/// <remarks>
/// Implements Unit of a Work pattern to:
/// <para>- Manage transaction lifecycle</para>
/// <para>- Ensure atomic operations across repositories</para>
/// <para>- Centralize database change tracking</para>
/// </remarks>
/// <param name="dataContext">Entity Framework Core database context</param>
/// <seealso href="https://www.c-sharpcorner.com/UploadFile/b1df45/unit-of-work-in-repository-pattern/"></seealso>
/// <seealso href="https://stackoverflow.com/questions/54671253/registering-iunitofwork-as-service-in-net-core"/>
public class UnitOfWork(DataContext dataContext) : IUnitOfWork
{
    // subcontextTransaction is used to manage transactions, and
    // we are setting it to null initially
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Initiates a new database transaction
    /// </summary>
    /// <remarks>
    /// Creates an explicit transaction if none exists. Later calls will
    /// reuse the existing transaction scope until commit/rollback.
    /// </remarks>
    public async Task BeginTransactionAsync()
    {
        // Begin the transaction and store it in the _transaction variable
        // if it is null or not already created than create a new transaction
        _transaction ??= await dataContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits all changes made within the transaction scope
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no active transaction exists
    /// </exception>
    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            return;

        try
        {
            // Ensure all changes are detected
            dataContext.ChangeTracker.DetectChanges();

            await _transaction.CommitAsync();
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Aborts the current transaction and discards pending changes
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        // If there is no transaction, return
        if (_transaction == null)
            return;

        // Roll back the transaction
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    /// <summary>
    /// Refactored by Phind AI to return the affected records
    /// Persists all pending changes to the database
    /// Non-generic version Returns all entities as objects
    /// Best used when working with multiple entity types
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<object>> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            // 1. Capture relationship changes
            var relationshipChanges = dataContext.ChangeTracker
                .Entries<object>()
                .Where(e => e.Metadata.IsOwned()
                            || e.Metadata.GetForeignKeys().Any(fk => fk.IsOwnership))
                .ToList();

            // 2. Combine with entity changes
            var entityChanges = dataContext.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added
                    or EntityState.Modified
                    or EntityState.Deleted)
                .ToList();

            var allChanges = entityChanges.Concat(relationshipChanges).Distinct().ToList();

            await dataContext.SaveChangesAsync(cancellationToken);

            return allChanges;
        }
        catch (DbUpdateException ex)
        {
            // Enhanced error handling
            var entries = dataContext
                .ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .ToList();

            throw new Exception($"Save failed. Entries in conflict: {entries.Count}", ex);
        }
    }

    /// <summary>
    /// Refactored by Phind AI to return the affected records
    /// Persists all pending changes to the database
    /// Generic version Returns all entities as objects
    /// This method is used when working with a specific entity type
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>Number of affected records</returns>
    public async Task<IEnumerable<TEntity>> SaveChangesAsync<TEntity>(
        CancellationToken cancellationToken = default
    )
        where TEntity : class
    {
        try
        {
            // Same pattern for generic version
            // Get all entities that are currently being tracked
            var addedEntries = dataContext
                .ChangeTracker.Entries()
                .Where(e =>
                    e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted
                )
                .ToList();

            // Save the changes to the database
            await dataContext.SaveChangesAsync(cancellationToken);
            // JetBrains Rider refactored the return type to IEnumerable<TEntity>
            return (IEnumerable<TEntity>)addedEntries;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while saving changes", ex);
        }
    }

    public string GetDebugView()
    {
        return dataContext.ChangeTracker.DebugView.LongView;
    }

    /// <summary>
    /// Retrieves all entities currently being tracked by the DbContext
    /// Generated by Phind AI
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IEnumerable<TEntity> GetTrackedEntities<TEntity>()
        where TEntity : class
    {
        // Return all entities that are currently being tracked
        return dataContext
            .ChangeTracker.Entries<TEntity>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity);
    }

    /// <summary>
    /// Releases managed database resources
    /// </summary>
    /// <remarks>
    /// Disposes both the active transaction (if any) and the DbContext.
    /// Should typically be called at the end of a request lifecycle.
    /// </remarks>
    public void Dispose()
    {
        // Dispose of the transaction if it exists
        _transaction?.Dispose();

        // Dispose the data context
        dataContext.Dispose();

        // Suppress finalization.
        // We don't need the destructor to run after
        // we've manually disposed of the resources.
        GC.SuppressFinalize(this);
    }
}