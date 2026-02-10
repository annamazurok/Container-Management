using FluentValidation;

namespace Application.Histories.Commands;

public class DeleteHistoryCommandValidator : AbstractValidator<DeleteHistoryCommand>
{
    public DeleteHistoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}