using FluentValidation;

namespace Diploma.DTO.Controllers.UserController.Requests;

public class AddUserDtoValidator : AbstractValidator<AddUserDto>
{
    public AddUserDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.NotifyLevel).NotNull().NotEmpty();
    }
}