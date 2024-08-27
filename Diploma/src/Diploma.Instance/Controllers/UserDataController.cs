using Diploma.Domain.Models;
using Diploma.Domain.Repositories.Users;
using Diploma.DTO.Controllers.UserController.Requests;
using Diploma.Instance.Configuration.Mappers;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable All

namespace Diploma.Instance.Controllers;

[ApiController]
[Route("v1/users")]
public class UserDataController : ControllerBase
{
    private readonly ILogger<UserDataController> _logger;
    private readonly IUsersRepository _usersRepository;

    public UserDataController(ILogger<UserDataController> logger,
                              IUsersRepository usersRepository)
    {
        _logger = logger;
        _usersRepository = usersRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на получение пользователей");
        var users = await _usersRepository.GetAsync(cancellationToken);

        return await Task.FromResult(Ok(users.Select(x => UsersMapper.MapFromUser(x))));
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody]AddUserDto model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на добавление пользователя");

        var validator = new AddUserDtoValidator();
        var validationResult = validator.Validate(model);
        if (!validationResult.IsValid)
        {
            return BadRequest();
        }

        var userToCreate = new UserInfo()
        {
            Email = model.Email,
            Name = model.Name,
            NotifyLevel = model.NotifyLevel,
            CreateAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _usersRepository.AddAsync(userToCreate, cancellationToken);

        return Created();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на удаление данных о пользователе с id {id}");

        var user = await _usersRepository.GetByPredicateAsync(x => x.Id == id, cancellationToken);
        if (user is null)
        {
            return BadRequest();
        }

        await _usersRepository.DeleteAsync(user, cancellationToken);

        return Ok();
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateAsync([FromBody]UpdateUserDto model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поступил запрос на обновление данных о пользователе с id {id}");
        
        var user = await _usersRepository.GetByPredicateAsync(x => x.Id == model.Id, cancellationToken);
        if (user is null)
        {
            return BadRequest();
        }

        user.NotifyLevel = model.NotifyLevel;
        user.Email = model.Email;
        user.Name = model.Name;

        await _usersRepository.UpdateAsync(user, cancellationToken);

        return Ok();
    }
}