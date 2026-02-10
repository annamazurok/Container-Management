using Api.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;

namespace Api.Modules.Validators.ContainerType;

public class CreateContainerTypeDtoValidator :  AbstractValidator<CreateContainerTypeDto>
{
    public CreateContainerTypeDtoValidator()
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