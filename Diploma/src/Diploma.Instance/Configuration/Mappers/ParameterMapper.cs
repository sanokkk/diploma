using Diploma.Domain.Models;
using Diploma.DTO.Controllers.UserController.Requests;
using Diploma.DTO.Controllers.UserController.Responses;

namespace Diploma.Instance.Configuration.Mappers;

public static class ParameterMapper
{
    public static GetParameterDto MapFromParameter(Parameter parameter)
    {
        return new GetParameterDto(
            parameter.Id.ToString(),
            parameter.ParameterType.ParameterType.ToString(),
            parameter.MinValue,
            parameter.MaxValue);
    }

    public static Parameter MapFromCreateParameterDto(CreateParameterDto dto)
    {
        return new Parameter()
        {
            ParameterType = new ParameterTypeWrapper()
            {
                Id = dto.ParameterTypeId
            },
            MaxValue = dto.MaxValue,
            MinValue = dto.MinValue,
            CreateAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }
}
