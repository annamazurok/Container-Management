using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.ProductType;

public class CreateProductTypeDtoValidator : AbstractValidator<CreateProductTypeDto>
{
    public CreateProductTypeDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(150);
    }
}