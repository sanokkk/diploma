using Diploma.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Domain.Repositories.ParameterType;

public class ParameterTypeRepository : RepositoryBase<ParameterTypeWrapper>, IParameterTypeRepository
{
    public ParameterTypeRepository(DiplomaContext context) : base(context)
    { }

    public async Task<IReadOnlyCollection<ParameterTypeWrapper>> GetParametersAsync(UnitType type, CancellationToken cancellationToken)
    {
        return await _context.ParameterTypes.Where(x => x.UnitType == type).ToListAsync(cancellationToken);
    }
}
