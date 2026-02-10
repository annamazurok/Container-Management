using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Unit;

public class CreateUnitDtoValidator :  AbstractValidator<CreateUnitDto>
{
    public CreateUnitDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(150);

        RuleFor(x => x.UnitType)
            .IsInEnum();
    }
}