using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Containers.Commands;

public class CreateContainerCommand : IRequest<Either<BaseException, Container>>
{
    public required string Name { get; init; }
    public required string Code { get; init; }
    public required int TypeId { get; init; }
    public required int ProductId { get; init; }
    public required int Quantity { get; init; }
    public required int UnitId { get; init; }
    public string? Notes { get; init; }
}

public class CreateContainerCommandHandler(
    IRepository<Container> containerRepository,
    IContainerQueries containerQueries,
    IContainerTypeQueries containerTypeQueries,
    //IProductQueries productQueries,
    IUnitQueries unitQueries) : IRequestHandler<CreateContainerCommand, Either<BaseException, Container>>
{
    public async Task<Either<BaseException, Container>> Handle(CreateContainerCommand request, CancellationToken cancellationToken)
    {
        var existingContainer = await containerQueries.GetByNameAsync(request.Name, cancellationToken);

        return await existingContainer.MatchAsync(
            c => new ContainerAlreadyExistException(c.Id),
            () => CheckDependencies(request, cancellationToken)
                .BindAsync(_ => CreateEntity(request, cancellationToken)));
    }

    private async Task<Either<BaseException, Unit>> CheckDependencies(
        CreateContainerCommand request, CancellationToken cancellationToken)
    {
        var containerType = await containerTypeQueries.GetByIdAsync(request.TypeId, cancellationToken);
        if (containerType.IsNone)
            return new ContainerTypeNotFoundException(request.TypeId);

        //var product = await productQueries.GetByIdAsync(request.ProductId, cancellationToken);
        // if (product.IsNone)
        //     return new ProductNotFoundException(request.ProductId);

        var unit = await unitQueries.GetByIdAsync(request.UnitId, cancellationToken);
        if (unit.IsNone)
            return new UnitNotFoundException(request.UnitId);

        return Unit.Default;
    }

    private async Task<Either<BaseException, Container>> CreateEntity(
        CreateContainerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var container = await containerRepository.CreateAsync(
                Container.New(
                    request.Name,
                    request.Code,
                    request.TypeId,
                    request.ProductId,
                    request.Quantity,
                    request.UnitId,
                    request.Notes,
                    1), // TODO: Replace with actual userId from ICurrentUserService
                cancellationToken);

            return container;
        }
        catch (Exception ex)
        {
            return new UnhandledContainerException(0, ex);
        }
    }
}