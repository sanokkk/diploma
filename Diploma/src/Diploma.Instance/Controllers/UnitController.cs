using System.Linq.Expressions;
using System.Xml.Serialization;
using Diploma.Domain.Repositories.ParameterType;
using Diploma.Domain.Repositories.Units;
using Diploma.DTO.Controllers.UserController.Requests;
using Diploma.Instance.Configuration.Mappers;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable All

namespace Diploma.Instance.Controllers;

[ApiController]
[Route("v1/unit")]
public class UnitController : ControllerBase
{
    private readonly IUnitRepository _unitRepository;
    private readonly IParameterTypeRepository _parameterTypeRepository;
    private readonly ILogger<UnitController> _logger;

    public UnitController(IUnitRepository unitRepository,
                          IParameterTypeRepository parameterTypeRepository,
                          ILogger<UnitController> logger)
    {
        _unitRepository = unitRepository;
        _logger = logger;
        _parameterTypeRepository = parameterTypeRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на просмотр всех узлов");
        var unitsFromDb = await _unitRepository.GetWithInclude(false, x => x != null, cancellationToken);

        return Ok(unitsFromDb.Select(UnitMapper.MapFromUnit));
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody]CreateUnitDto model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на добавление узла");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList());
        }

        try
        {
            await _unitRepository.AddAsync(UnitMapper.MapFromCreateUnitDto(model), cancellationToken);
        }
        catch (Exception e)
        {
        }

        return new CreatedResult();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Поступил запрос на удаление узла {id}");

        var unit = await _unitRepository.GetByPredicateAsync(x => x.Id == id, cancellationToken);
        if (unit is null)
        {
            return BadRequest();
        }

        await _unitRepository.DeleteAsync(unit, cancellationToken);

        return Ok();
    }

    [HttpPost("file")]
    public async Task<ActionResult> PostFromFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на добавление узлов из файла");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, cancellationToken);
        ms.Position = 0;

        var serializer = new XmlSerializer(typeof(CreateUnitsFromFileDto));
        var model = (CreateUnitsFromFileDto)serializer.Deserialize(ms)!;

        var createUnitDtos = await ConvertToCreateUnitDtos(model, cancellationToken);
        foreach (var unit in createUnitDtos)
        {
            await _unitRepository.AddAsync(UnitMapper.MapFromCreateUnitDto(unit), cancellationToken);
        }
        
        return Ok(model);
    }

    private async Task<IReadOnlyCollection<CreateUnitDto>> ConvertToCreateUnitDtos(CreateUnitsFromFileDto model, CancellationToken cancellationToken)
    {
        var createUnitDtos = new List<CreateUnitDto>();

        foreach (var unit in model.Units)
        {
            var parameters = new List<CreateParameterDto>();
            foreach (var param in unit.Parameters)
            {
                var parameterId = await _parameterTypeRepository.GetByPredicateAsync(
                    x => x.ParameterType == param.ParameterName
                         && x.UnitType == unit.UnitType,
                    cancellationToken,
                    true);

                var createParameterDto = new CreateParameterDto(parameterId.Id, param.MinValue, param.MaxValue);
                parameters.Add(createParameterDto);
            }
            
            createUnitDtos.Add(new CreateUnitDto(unit.UnitType, unit.Name, parameters.ToArray()));
        }

        return createUnitDtos;
    }
}