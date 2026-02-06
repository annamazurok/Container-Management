using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Histories.Commands;

public class CreateHistoryCommand : IRequest<Either<BaseException, History>>
{
    public required int ContainerId { get; init; }
    public required int Quantity { get; init; }
    public required int UnitId { get; init; }
    public required int ProductId { get; init; }
    public required Status ActionType { get; init; }
    public required DateTime FromDate { get; init; }
}

public class CreateHistoryCommandHandler(
    IRepository<History> historyRepository,
    IContainerQueries containerQueries,
    //IProductQueries productQueries,
    IUnitQueries unitQueries) : IRequestHandler<CreateHistoryCommand, Either<BaseException, History>>
{
    public async Task<Either<BaseException, History>> Handle(CreateHistoryCommand request, CancellationToken cancellationToken)
    {
        return await CheckDependencies(request, cancellationToken)
            .BindAsync(_ => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, Unit>> CheckDependencies(
        CreateHistoryCommand request, CancellationToken cancellationToken)
    {
        var container = await containerQueries.GetByIdAsync(request.ContainerId, cancellationToken);
        if (container.IsNone)
            return new ContainerNotFoundException(request.ContainerId);

        // var product = await productQueries.GetByIdAsync(request.ProductId, cancellationToken);
        // if (product.IsNone)
        //     return new ProductNotFoundException(request.ProductId);

        var unit = await unitQueries.GetByIdAsync(request.UnitId, cancellationToken);
        if (unit.IsNone)
            return new UnitNotFoundException(request.UnitId);

        return Unit.Default;
    }

    private async Task<Either<BaseException, History>> CreateEntity(
        CreateHistoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var history = await historyRepository.CreateAsync(
                History.New(
                    0,
                    request.ContainerId,
                    request.Quantity,
                    request.UnitId,
                    request.ProductId,
                    request.ActionType,
                    request.FromDate,
                    1), // TODO: Replace with actual userId from ICurrentUserService
                cancellationToken);

            return history;
        }
        catch (Exception ex)
        {
            return new UnhandledHistoryException(0, ex);
        }
    }
}