using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.ProductType;

public class UpdateProductTypeDtoValidator : AbstractValidator<ProductTypeDto>
{
    public UpdateProductTypeDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(150);
    }
}