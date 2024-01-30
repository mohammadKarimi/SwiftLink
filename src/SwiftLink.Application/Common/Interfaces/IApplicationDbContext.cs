using SwiftLink.Domain.Common;

namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// This interface facilitates communication between the application layer, infrastructure, and the DbContext.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Save all entities in to database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}