using Diploma.Domain.Models;

namespace Diploma.DTO.Controllers.UserController.Responses;

public sealed record GetUsersDto(Guid Id, string Email, string Name, ConditionState NotifyLevel);