using FluentValidation;

namespace Application.Units.Commands;

public class CreateUnitCommandValidator :  AbstractValidator<CreateUnitCommand>
{
    public CreateUnitCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(150);

        RuleFor(x => x.UnitType)
            .IsInEnum();
    }
}