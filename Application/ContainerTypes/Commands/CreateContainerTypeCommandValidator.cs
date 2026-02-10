using FluentValidation;

namespace Application.ContainerTypes.Commands;

public class CreateContainerTypeCommandValidator :  AbstractValidator<CreateContainerTypeCommand>
{
    public CreateContainerTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(150);

        RuleFor(x => x.Volume)
            .GreaterThan(0);

        RuleFor(x => x.UnitId)
            .GreaterThan(0);
    }
}