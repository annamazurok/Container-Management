using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Container;

public class CreateContainerDtoValidator :  AbstractValidator<CreateContainerDto>
{
    public CreateContainerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(150);

        RuleFor(x => x.TypeId)
            .GreaterThan(0);

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .When(x => x.ProductId.HasValue);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Quantity.HasValue);

        RuleFor(x => x.UnitId)
            .GreaterThan(0)
            .When(x => x.UnitId.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(500);
    }
}