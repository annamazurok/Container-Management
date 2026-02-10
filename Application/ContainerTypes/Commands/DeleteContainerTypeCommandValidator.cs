using System.Drawing;
using FluentValidation;

namespace Application.ContainerTypes.Commands;

public class DeleteContainerTypeCommandValidator :  AbstractValidator<DeleteContainerTypeCommand>
{
    public DeleteContainerTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}