using FluentValidation;

namespace Application.Units.Commands;

public class DeleteUnitCommandValidator :  AbstractValidator<DeleteUnitCommand>
{
    public DeleteUnitCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}