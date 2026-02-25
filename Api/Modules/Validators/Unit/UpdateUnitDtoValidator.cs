using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Unit;

public class UpdateUnitDtoValidator :  AbstractValidator<UnitDto>
{
    public UpdateUnitDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(150);

        RuleFor(x => x.UnitType)
            .IsInEnum();
    }
}