using Diploma.Domain.Models;
using Diploma.DTO.Controllers.UserController.Requests;

namespace Diploma.Instance.Configuration.Mappers;

public static class ParameterTypeMapper
{
    public static ParameterTypeWrapper MapFromCreateParameterType(AddParameterTypeDto dto)
    {
        return new ParameterTypeWrapper()
        {
            Weight = dto.Weight,
            IsStatic = dto.IsStatic,
            ParameterType = dto.Name,
            UnitType = dto.UnitType
        };
    }
}
