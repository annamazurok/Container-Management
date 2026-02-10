using FluentValidation;

namespace Application.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
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