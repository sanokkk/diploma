using Diploma.Domain.Models;

namespace Diploma.DTO.Controllers.UserController.Requests;

public sealed record UpdateUserDto(
    Guid Id, 
    string Name, 
    string Email,
    ConditionState NotifyLevel);

