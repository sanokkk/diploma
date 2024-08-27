using Diploma.Domain.Models;
using Diploma.DTO.Controllers.UserController.Requests;
using Diploma.DTO.Controllers.UserController.Responses;

namespace Diploma.Instance.Configuration.Mappers;

public static class UnitMapper
{
    public static GetUnitDto MapFromUnit(Unit unit)
    {
        return new GetUnitDto(
            unit.Id.ToString(),
            unit.Name,
            unit.UnitType.ToString(),
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            unit.Parameters is null ? new GetParameterDto[]{} : unit.Parameters.Select(ParameterMapper.MapFromParameter).ToList()
        );
    }

    public static Unit MapFromCreateUnitDto(CreateUnitDto dto)
    {
        return new Unit()
        {
            Name = dto.Name,
            Parameters = dto.Parameters.Select(ParameterMapper.MapFromCreateParameterDto).ToList(),
            UnitType = dto.Type,
            State = ConditionState.Хорошее,
            CreateAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };
    }
}