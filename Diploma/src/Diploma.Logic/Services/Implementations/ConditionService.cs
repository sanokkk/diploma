using System.Text;
using Diploma.Domain.Models;
using Diploma.Domain.Repositories.Units;
using Diploma.DTO.Reports;
using Diploma.DTO.Tcp.Models;
using Diploma.DTO.Tcp.Responses;
using Diploma.Logic.Helpers;
using Diploma.Logic.Metrics;
using Diploma.Logic.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Diploma.Logic.Services.Implementations;

public class ConditionService : IConditionService
{
    private readonly IUnitRepository _unitRepository;
    private readonly INotifyService _notifyService;
    private readonly ILogger<ConditionService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IReportService _reportService;
    private readonly MetricsCollector _metricsCollector;

    public ConditionService(IUnitRepository unitRepository,
                            IMemoryCache memoryCache,
                            INotifyService notifyService,
                            IReportService reportService,
                            MetricsCollector metricsCollector,
                            ILogger<ConditionService> logger)
    {
        _unitRepository = unitRepository;
        _memoryCache = memoryCache;
        _notifyService = notifyService;
        _reportService = reportService;
        _logger = logger;
        _metricsCollector = metricsCollector;
    }

    public async Task<ConditionResponse> PerformDataFromPlcAsync(TcpMessage[] message, CancellationToken cancellationToken)
    {
        var messagesToReport = new List<string>();

        var unitNamesInMessage = message.Select(x => x.Name);
        var unitsNotInMessageReports = (await _unitRepository.GetEnumerableByPredicateAsync(x => !unitNamesInMessage.Contains(x.Name), cancellationToken))
            .Select(x => $"Не получена информация с узла {x.Name}, проверьте подключенные датчики и ПЛК");
        
        messagesToReport.AddRange(unitsNotInMessageReports);

        var response = new List<UnitResponse>();
        var report = new List<UnitReport>();

        foreach (var unitMessage in message)
        {
            var formulaForReport = new StringBuilder("I = (");

            var unitResponse = await ComposeParametersForCalculationsAsync(unitMessage, 
                                                                           messagesToReport,
                                                                           formulaForReport,
                                                                           cancellationToken);

            await SaveCalculationsAsync(unitResponse, cancellationToken);

            var dataForReport = GetDataForReport(unitResponse, unitMessage, formulaForReport.ToString().Replace("+ )", ")"));
            report.Add(dataForReport);

            response.Add(unitResponse);
        }

        await _notifyService.NotifyAsync(messagesToReport, cancellationToken);
        _reportService.GenerateReport(report);

        return new ConditionResponse(response);
    }

    public async Task<ConditionResponse> GetLastUnitDataAsync(CancellationToken cancellationToken)
    {
        var dbUnits = await _unitRepository.GetAsync(cancellationToken);

        var unitsResponse = dbUnits.Select(x => new UnitResponse(x.Name, (int)x.State)).ToList();

        return new ConditionResponse(unitsResponse);
    }

    private async Task SaveCalculationsAsync(UnitResponse unitResponse, CancellationToken cancellationToken)
    {
        var dbUnit = await _unitRepository.GetByPredicateAsync(x => x.Name == unitResponse.UnitName, cancellationToken);

        var newState = EnumHelper.GetStateByIndex(unitResponse.Index);
        dbUnit.State = newState;
        await _unitRepository.UpdateAsync(dbUnit, cancellationToken);
    }

    private async Task<UnitResponse> ComposeParametersForCalculationsAsync(TcpMessage message, 
                                                                           List<string> reports,
                                                                           StringBuilder formulaBuilder,
                                                                           CancellationToken cancellationToken)
    {
        var unitByName = (await _unitRepository.GetWithInclude(false, x => x.Name == message.Name, cancellationToken)).First();
        int weightSum = 0;

        var nonStaticIndex = CalculateNonStaticIndex(message, reports, unitByName, ref weightSum, formulaBuilder);

        var staticIndex = CalculateStaticIndex(message, reports, unitByName, ref weightSum, formulaBuilder);

        var calculationResult = (staticIndex + nonStaticIndex) / weightSum;
        var index = ComposeIndex(calculationResult);
        formulaBuilder.Append($") / {weightSum} = {calculationResult}");

        return new UnitResponse(unitByName.Name, index);
    }

    private int ComposeIndex(double calculatedResult)
    {
        if (calculatedResult < 0.2) return 1;
        if (calculatedResult < 0.4) return 2;
        if (calculatedResult < 0.6) return 3;
        if (calculatedResult < 0.8) return 4;
        return 5;
    }

    private UnitReport GetDataForReport(UnitResponse unitResponse,
                                        TcpMessage plcData,
                                        string formula)
    {
        return new UnitReport(
            unitResponse.UnitName,
            plcData.Parameters.Select(x => new ParameterReport(x.ParameterName, x.Value)).ToList(),
            formula,
            unitResponse.Index);
    }

    private double CalculateStaticIndex(TcpMessage message, List<string> reports, Unit unitByName, ref int weightSum, StringBuilder formulaBuilder)
    {
        double staticIndex = 0;
        var staticParameters = unitByName.Parameters.Where(x => x.ParameterType.IsStatic).ToArray();

        foreach (var staticParameter in staticParameters)
        {
            var parameter = message.Parameters.First(x => x!.ParameterName == staticParameter.ParameterType.ParameterType);

            if (parameter is null)
            {
                CheckParameterConsistency(reports, unitByName, staticParameter);
                continue;
            }
            
            var name = $"{staticParameter.Unit.Name} {staticParameter.ParameterType.ParameterType!}"; 
            _logger.LogInformation($"Выставляю метрику {name} - {parameter.Value}");
            _metricsCollector.Set(name, parameter.Value);

            weightSum += staticParameter.ParameterType.Weight;

            _memoryCache.Set($"{unitByName.Name}:{staticParameter.ParameterType.ParameterType}", 0);

            staticIndex += (double)CalculateIndex(parameter.Value,
                                          staticParameter.MinValue,
                                          staticParameter.MaxValue,
                                          staticParameter.ParameterType.Weight,
                                          formulaBuilder,
                                          true,
                                          reports,
                                          unitByName.Name,
                                          parameter.ParameterName);
        }

        return staticIndex;
    }

    private double CalculateNonStaticIndex(TcpMessage message, 
                                           List<string> reports,
                                           Unit unitByName,
                                           ref int weightSum,
                                           StringBuilder formulaBuilder)
    {
        double nonStaticIndex = 0;
        var nonStaticParameters = unitByName.Parameters.Where(x => !x.ParameterType.IsStatic).ToArray();
        foreach (var nonStaticParameter in nonStaticParameters)
        {
            var parameter = message.Parameters.First(x => x!.ParameterName == nonStaticParameter.ParameterType.ParameterType);
            
            if (parameter is null)
            {
                CheckParameterConsistency(reports, unitByName, nonStaticParameter);
                continue;
            }

            var name = $"{nonStaticParameter.Unit.Name} {nonStaticParameter.ParameterType.ParameterType!}"; 
            _logger.LogInformation($"Выставляю метрику {name} - {parameter.Value}");
            _metricsCollector.Set(name, parameter.Value);

            weightSum += nonStaticParameter.ParameterType.Weight;

            _memoryCache.Set($"{unitByName.Name}:{nonStaticParameter.ParameterType.ParameterType}", 0);

            nonStaticIndex += CalculateIndex(parameter.Value,
                                             nonStaticParameter.MinValue,
                                             nonStaticParameter.MaxValue,
                                             nonStaticParameter.ParameterType.Weight,
                                             formulaBuilder);
        }

        return nonStaticIndex;
    }

    private void CheckParameterConsistency(List<string> reports, Unit unitByName, Parameter parameter)
    {
        if (_memoryCache.TryGetValue($"{unitByName.Name}:{parameter.ParameterType.ParameterType}", out int timesWithoutValue))
        {
            reports.Add(
                $"Параметр {parameter.ParameterType.ParameterType} узла {unitByName.Name} имеет значение состоятельности уже {timesWithoutValue} итераций." +
                $" Проверьте корректность подключения датчика или работу ПЛК (либо процесс неактивен и параметр не должен считываться)");
        }
    }

    private double CalculateIndex(double currentValue, 
                                  double minValue,
                                  double maxValue,
                                  int weight,
                                  StringBuilder formulaBuilder,
                                  bool isStatic=false,
                                  List<string>? reports = null,
                                  string unitName = "",
                                  string parameterName = "")
    {
        double normalizedValue = 0;
        if (isStatic && currentValue == minValue && currentValue == maxValue)
        {
            normalizedValue = 0.1;
        }
        else
        {
            normalizedValue = Math.Round((currentValue - minValue) / (maxValue - minValue), 4);
        }
        

        if (isStatic)
        {
            if (normalizedValue > 1 || normalizedValue < 0)
            {
                reports?.Add($"Статический параметр {parameterName} узла {unitName} имеет отклонение от нормы. Незамедлительно проверьте оборудование");

                formulaBuilder.Append($"1 * {weight} + ");
                return 1 * weight;
            }
        }

        formulaBuilder.Append($"{normalizedValue} * {weight} + ");
        var result = normalizedValue * weight;
        return result;
    }
}