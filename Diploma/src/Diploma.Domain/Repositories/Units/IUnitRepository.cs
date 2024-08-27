using System.Linq.Expressions;
using Diploma.Domain.Models;

namespace Diploma.Domain.Repositories.Units;

public interface IUnitRepository : IRepository<Unit>
{
    Task<IReadOnlyCollection<Unit>> GetWithInclude(bool asNoTracking, Expression<Func<Unit, bool>> predicate, CancellationToken cancellationToken);
    new Task AddAsync(Unit model, CancellationToken cancellationToken);

    Task<IEnumerable<Unit>> GetEnumerableByPredicateAsync(Expression<Func<Unit, bool>> expression, CancellationToken cancellationToken);
}
