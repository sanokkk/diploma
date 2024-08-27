using System.Linq.Expressions;

namespace Diploma.Domain.Repositories;

public interface IRepository<TEntity>
    where TEntity: class
{
    Task AddAsync(TEntity model, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity model, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TEntity>> GetAsync(CancellationToken cancellationToken);
    Task<TEntity> GetByPredicateAsync(Expression<Func<TEntity, bool>> expression,
                                      CancellationToken cancellationToken,
                                      bool noTracking = false);
    Task UpdateAsync(TEntity model, CancellationToken cancellationToken);
}