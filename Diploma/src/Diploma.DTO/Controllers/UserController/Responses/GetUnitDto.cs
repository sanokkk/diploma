#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Diploma.DTO.Controllers.UserController.Responses;

public sealed record GetUnitDto(
    string Id,
    string Name,
    string UnitType,
    IReadOnlyCollection<GetParameterDto> Parameters);
