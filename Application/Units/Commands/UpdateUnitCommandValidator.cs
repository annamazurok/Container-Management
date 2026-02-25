using FluentValidation;

namespace Application.Units.Commands;

public class UpdateUnitCommandValidator :  AbstractValidator<UpdateUnitCommand>
{
    public UpdateUnitCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(150);

        RuleFor(x => x.UnitType)
            .IsInEnum();
    }
}