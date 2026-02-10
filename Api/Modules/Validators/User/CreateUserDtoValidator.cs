using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.User;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(x => x.FathersName)
            .MinimumLength(2)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.FathersName));

        RuleFor(x => x.RoleId)
            .GreaterThan(0);
    }
}