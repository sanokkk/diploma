using Diploma.Domain.Models;
using Diploma.Domain.Repositories.ParameterType;
using Diploma.Logic.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Instance.Controllers;

[ApiController]
[Route("v1/enum")]
public class EnumController : ControllerBase
{
    private readonly IParameterTypeRepository _parameterTypeRepository;
    private readonly ILogger<EnumController> _logger;

    public EnumController(IParameterTypeRepository parameterTypeRepository, ILogger<EnumController> logger)
    {
        _parameterTypeRepository = parameterTypeRepository;
        _logger = logger;
    }
    
    /// <param name="type">Тип узла</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("parameters/{type}")]
    public async Task<ActionResult> GetParametersAsync([FromRoute]UnitType type, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Поступил запрос на получение типов параметров для {type:G}");
        
        var parameters = await _parameterTypeRepository.GetParametersAsync(type, cancellationToken);

        return Ok(parameters);
    }

    [HttpGet("units")]
    public async Task<ActionResult> GetUnitTypesAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult<ActionResult>(Ok(Enum.GetNames(typeof(UnitType)).Select(EnumHelper.GetNormalizedName)));
    }

    [HttpGet("conditions")]
    public async Task<ActionResult> GetConditionTypes(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Поступил запрос на получение типов состояния оборудования");

        var names = Enum.GetNames(typeof(ConditionState));
        
        return await Task.FromResult(Ok(names));
    }
}
