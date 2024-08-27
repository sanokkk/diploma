using System.Net.Mime;
using Diploma.Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Diploma.Instance.Controllers;

[ApiController]
[Route("v1/condition")]
public class ConditionController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IConditionService _conditionService;
    private readonly ILogger<ConditionController> _logger;

    public ConditionController(IReportService reportService,
                               IConditionService conditionService,
                               ILogger<ConditionController> logger)
    {
        _reportService = reportService;
        _conditionService = conditionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetConditionsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на получения индексов узлов из БД");

        var response = await _conditionService.GetLastUnitDataAsync(cancellationToken);

        return Ok(response);
    }

    [HttpPost("report")]
    public async Task<ActionResult> GetLastReportAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на получение последнего файла-отчета о вычислениях");

        Response.Clear();
        Response.Headers.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.Headers.ContentDisposition = "attachment; filename=result.xlsx; filename*=UTF-8\'result.xlsx\'";
        var ms = new MemoryStream();
        await _reportService.GetLastReportAsync(ms, cancellationToken);

        ms.Position = 0;

        return new FileStreamResult(ms, "application/octet-stream");
        return new EmptyResult();
    }
}
