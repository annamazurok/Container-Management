using FluentValidation;

namespace Application.ProductTypes.Commands;

public class DeleteProductTypeCommandValidator : AbstractValidator<DeleteProductTypeCommand>
{
    public DeleteProductTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}