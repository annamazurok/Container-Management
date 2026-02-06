using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Containers.Commands;

public class UpdateContainerCommand : IRequest<Either<BaseException, Container>>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int TypeId { get; init; }
    public required int ProductId { get; init; }
    public required int Quantity { get; init; }
    public required int UnitId { get; init; }
    public string? Notes { get; init; }
}

public class UpdateContainerCommandHandler(
    IRepository<Container> containerRepository,
    IContainerQueries containerQueries,
    IContainerTypeQueries containerTypeQueries,
    IUnitQueries unitQueries) : IRequestHandler<UpdateContainerCommand, Either<BaseException, Container>>
{
    public async Task<Either<BaseException, Container>> Handle(UpdateContainerCommand request, CancellationToken cancellationToken)
    {
        var container = await containerQueries.GetByIdAsync(request.Id, cancellationToken);

        return await container.MatchAsync(
            c => CheckDuplicates(c.Id, request.Name, cancellationToken)
                .BindAsync(_ => CheckDependencies(request, cancellationToken)
                    .BindAsync(_ => UpdateEntity(request, c, cancellationToken))),
            () => new ContainerNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Container>> UpdateEntity(
        UpdateContainerCommand request,
        Container container,
        CancellationToken cancellationToken)
    {
        try
        {
            container.UpdateDetails(
                request.Name,
                request.TypeId,
                request.ProductId,
                request.Quantity,
                request.UnitId,
                request.Notes,
                1); // TODO: Replace with actual userId from ICurrentUserService

            return await containerRepository.UpdateAsync(container, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledContainerException(container.Id, ex);
        }
    }

    private async Task<Either<BaseException, Unit>> CheckDependencies(
        UpdateContainerCommand request, CancellationToken cancellationToken)
    {
        var containerType = await containerTypeQueries.GetByIdAsync(request.TypeId, cancellationToken);
        if (containerType.IsNone)
            return new ContainerTypeNotFoundException(request.TypeId);

        var unit = await unitQueries.GetByIdAsync(request.UnitId, cancellationToken);
        if (unit.IsNone)
            return new UnitNotFoundException(request.UnitId);

        return Unit.Default;
    }

    private async Task<Either<BaseException, Unit>> CheckDuplicates(
        int currentContainerId,
        string name,
        CancellationToken cancellationToken)
    {
        var container = await containerQueries.GetByNameAsync(name, cancellationToken);

        return container.Match<Either<BaseException, Unit>>(
            c => c.Id.Equals(currentContainerId) ? Unit.Default : new ContainerAlreadyExistException(c.Id),
            () => Unit.Default);
    }
}