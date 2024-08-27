using System.Text;
using Diploma.DTO.Tcp.Models;
using Diploma.Instance.Hubs.Interfaces;
using Diploma.Logic.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Diploma.Instance.Hubs;

public class PlcHub : Hub<ITcpClient>
{
    private readonly ILogger<PlcHub> _logger;
    private readonly IConditionService _conditionService;
    private const int TimeoutPeriod = 10;

    public PlcHub(IConditionService conditionService, ILogger<PlcHub> logger)
    {
        _conditionService = conditionService;
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation($"подключился клиент {Context.ConnectionId}");

        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation($"отключился клиент {Context.ConnectionId}");

        return Task.CompletedTask;
    }

    public async Task GetPlcMessage(string message)
    {
        _logger.LogInformation($"получил информацию с ПЛК: {message}");

        try
        {
            var convertedData = JsonSerializer.Deserialize<TcpMessage[]>(message);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(TimeoutPeriod));
            var response = await _conditionService.PerformDataFromPlcAsync(convertedData!, cts.Token);

            
            var responseJson = JsonConvert.SerializeObject(response);
            await Clients.All.GetConditionData(responseJson);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(GetPlcMessage)}] error while getting and sending data from plc to client", ex.Message);
        }
    }
}