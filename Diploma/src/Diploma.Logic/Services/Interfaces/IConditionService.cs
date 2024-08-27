using Diploma.DTO.Tcp.Models;
using Diploma.DTO.Tcp.Responses;

namespace Diploma.Logic.Services.Interfaces;

public interface IConditionService
{
    Task<ConditionResponse> PerformDataFromPlcAsync(TcpMessage[] message, CancellationToken cancellationToken);
    Task<ConditionResponse> GetLastUnitDataAsync(CancellationToken cancellationToken);
}
