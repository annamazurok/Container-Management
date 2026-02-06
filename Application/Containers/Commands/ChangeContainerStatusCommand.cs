using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Containers.Commands;

public class ChangeContainerStatusCommand : IRequest<Either<BaseException, Container>>
{
    public required int Id { get; init; }
    public required Status Status { get; init; }
}

public class ChangeContainerStatusCommandHandler(
    IRepository<Container> containerRepository,
    IContainerQueries containerQueries) : IRequestHandler<ChangeContainerStatusCommand, Either<BaseException, Container>>
{
    public async Task<Either<BaseException, Container>> Handle(ChangeContainerStatusCommand request, CancellationToken cancellationToken)
    {
        var container = await containerQueries.GetByIdAsync(request.Id, cancellationToken);

        return await container.MatchAsync(
            c => UpdateEntity(request, c, cancellationToken),
            () => new ContainerNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Container>> UpdateEntity(
        ChangeContainerStatusCommand request,
        Container container,
        CancellationToken cancellationToken)
    {
        try
        {
            container.ChangeStatus(
                request.Status,
                1); // TODO: Replace with actual userId from ICurrentUserService

            return await containerRepository.UpdateAsync(container, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledContainerException(container.Id, ex);
        }
    }
}