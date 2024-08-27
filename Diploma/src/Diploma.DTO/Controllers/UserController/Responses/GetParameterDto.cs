namespace Diploma.DTO.Controllers.UserController.Responses;

public sealed record GetParameterDto(
    string Id,
    string ParameterType,
    double MinValue,
    double MaxValue);
