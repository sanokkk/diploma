using System.Linq.Expressions;
using Diploma.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Domain.Repositories.Units;

public sealed class UnitRepository : RepositoryBase<Unit>, IUnitRepository
{
    public UnitRepository(DiplomaContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Unit>> GetWithInclude(bool asNoTracking, Expression<Func<Unit, bool>> predicate, CancellationToken cancellationToken)
    {
        if (!asNoTracking)
        {
            return await _context.Units.Include(x => x.Parameters).ThenInclude(x => x.ParameterType).Where(predicate).AsNoTracking().ToListAsync(cancellationToken);
        }
        
        return await _context.Units.Include(x => x.Parameters).ThenInclude(x => x.ParameterType).Where(predicate).ToListAsync(cancellationToken);
    }

    public override async Task AddAsync(Unit model, CancellationToken cancellationToken)
    {
        foreach (var parameter in model.Parameters)
        {
            _context.Attach(parameter.ParameterType);
        }

        await _context.Units.AddAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}