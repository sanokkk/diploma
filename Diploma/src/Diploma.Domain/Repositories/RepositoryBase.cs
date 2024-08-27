using System.Collections.Immutable;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8603 // Possible null reference return.

namespace Diploma.Domain.Repositories;

public abstract class RepositoryBase<T> : IRepository<T>
    where T: class
{
    protected DiplomaContext _context;

    protected RepositoryBase(DiplomaContext context)
    {
        _context = context;
    }

    public virtual async Task AddAsync(T model, CancellationToken cancellationToken)
    {
        _context.Set<T>().Add(model);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T model, CancellationToken cancellationToken)
    {
        _context.Set<T>().Remove(model);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<T>> GetAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T> GetByPredicateAsync(Expression<Func<T, bool>> expression, 
                                             CancellationToken cancellationToken,
                                             bool noTracking = false)
    {
        if (noTracking)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);
        }
        return await _context.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task UpdateAsync(T model, CancellationToken cancellationToken)
    {
        _context.Set<T>().Update(model);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetEnumerableByPredicateAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().Where(expression).ToListAsync(cancellationToken);
    }
}