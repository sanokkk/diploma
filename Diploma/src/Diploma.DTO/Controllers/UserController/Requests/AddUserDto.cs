using Diploma.Domain.Models;

namespace Diploma.DTO.Controllers.UserController.Requests;

public sealed record AddUserDto(string Name, string Email, ConditionState NotifyLevel);