using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.User;

public class UpdateUserProfileDtoValidator : AbstractValidator<UpdateUserProfileDto>
{
    public UpdateUserProfileDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

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
    }
}