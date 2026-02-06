using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Histories.Commands;

public class UpdateHistoryCommand : IRequest<Either<BaseException, History>>
{
    public required int Id { get; init; }
    public required int ContainerId { get; init; }
    public required int Quantity { get; init; }
    public required int UnitId { get; init; }
    public required int ProductId { get; init; }
    public required Status ActionType { get; init; }
    public required DateTime FromDate { get; init; }
}

public class UpdateHistoryCommandHandler(
    IRepository<History> historyRepository,
    IHistoryQueries historyQueries,
    IContainerQueries containerQueries,
    //IProductQueries productQueries,
    IUnitQueries unitQueries) : IRequestHandler<UpdateHistoryCommand, Either<BaseException, History>>
{
    public async Task<Either<BaseException, History>> Handle(UpdateHistoryCommand request, CancellationToken cancellationToken)
    {
        var history = await historyQueries.GetByIdAsync(request.Id, cancellationToken);

        return await history.MatchAsync(
            h => CheckDependencies(request, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, h, cancellationToken)),
            () => new HistoryNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, History>> UpdateEntity(
        UpdateHistoryCommand request,
        History history,
        CancellationToken cancellationToken)
    {
        try
        {
            history.UpdateDetails(
                request.Quantity,
                request.FromDate,
                1); // TODO: Replace with actual userId from ICurrentUserService

            history.ChangeActionType(
                request.ActionType,
                1); // TODO: Replace with actual userId from ICurrentUserService

            return await historyRepository.UpdateAsync(history, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledHistoryException(history.Id, ex);
        }
    }

    private async Task<Either<BaseException, Unit>> CheckDependencies(
        UpdateHistoryCommand request, CancellationToken cancellationToken)
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
}