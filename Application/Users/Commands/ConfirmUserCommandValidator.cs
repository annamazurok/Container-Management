using FluentValidation;

namespace Application.Users.Commands;

public class ConfirmUserCommandValidator :  AbstractValidator<ConfirmUserCommand>
{
    public ConfirmUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}