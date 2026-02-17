using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Product;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(20);

        RuleFor(x => x.TypeId)
            .GreaterThan(0);

        RuleFor(x => x.Produced)
            .NotEmpty();

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(x => x.Produced)
            .When(x => x.ExpirationDate.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}