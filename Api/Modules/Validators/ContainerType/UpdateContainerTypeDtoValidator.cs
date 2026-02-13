using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.ContainerType;

public class UpdateContainerTypeDtoValidator : AbstractValidator<UpdateContainerTypeDto>
{
    public UpdateContainerTypeDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
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