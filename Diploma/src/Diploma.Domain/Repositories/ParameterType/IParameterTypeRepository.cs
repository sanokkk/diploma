using Diploma.Domain.Models;

namespace Diploma.Domain.Repositories.ParameterType;

public interface IParameterTypeRepository : IRepository<ParameterTypeWrapper>
{
    Task<IReadOnlyCollection<ParameterTypeWrapper>> GetParametersAsync(UnitType type, CancellationToken cancellationToken);
}
