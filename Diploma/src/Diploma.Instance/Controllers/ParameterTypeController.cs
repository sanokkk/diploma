using System.Xml.Serialization;
using Diploma.Domain.Models;
using Diploma.Domain.Repositories.ParameterType;
using Diploma.DTO.Controllers.UserController.Requests;
using Diploma.Instance.Configuration.Mappers;
using Diploma.Logic.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Diploma.Instance.Controllers;

[ApiController]
[Route("v1/parameter-type")]
public class ParameterTypeController : ControllerBase
{
    private readonly IParameterTypeRepository _parameterTypeRepository;
    private readonly ILogger<ParameterTypeController> _logger;
    private readonly MetricsCollector _metricsCollector;

    public ParameterTypeController(IParameterTypeRepository parameterTypeRepository, 
                                   MetricsCollector metricsCollector,
                                   ILogger<ParameterTypeController> logger)
    {
        _logger = logger;
        _parameterTypeRepository = parameterTypeRepository;
        _metricsCollector = metricsCollector;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody]AddParameterTypeDto model, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Поступил запрос на добавление типа параметра {model.Name} для типа узла {model.UnitType:G}");

        await _parameterTypeRepository.AddAsync(ParameterTypeMapper.MapFromCreateParameterType(model), cancellationToken);

        return new CreatedResult();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute]int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Поступил запрос на удаление типа параметра {id}");

        await _parameterTypeRepository.DeleteAsync(new ParameterTypeWrapper() { Id = id }, cancellationToken);

        return Ok();
    }

    [HttpPost("file")]
    public async Task<ActionResult> PostFromFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var name = file.FileName;

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, cancellationToken);

        ms.Position = 0;

        var serializer = new XmlSerializer(typeof(CreateParameterTypesDto));
        var resultModel = (CreateParameterTypesDto)serializer.Deserialize(ms)!;

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        foreach (var parameterToCreate in resultModel.Parameters)
        {
            await _parameterTypeRepository.AddAsync(ParameterTypeMapper.MapFromCreateParameterType(parameterToCreate), cancellationToken);
        }

        return Ok();
    }
}




