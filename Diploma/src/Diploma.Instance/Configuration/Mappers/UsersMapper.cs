using Diploma.Domain.Models;
using Diploma.DTO.Controllers.UserController.Responses;

namespace Diploma.Instance.Configuration.Mappers;

public static class UsersMapper
{
    public static GetUsersDto MapFromUser(UserInfo userInfo)
    {
        return new GetUsersDto(userInfo.Id, userInfo.Email, userInfo.Name, userInfo.NotifyLevel);
    }
}
