using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Histories.Commands;

public class ChangeHistoryActionTypeCommand : IRequest<Either<BaseException, History>>
{
    public required int Id { get; init; }
    public required Status ActionType { get; init; }
}

public class ChangeHistoryActionTypeCommandHandler(
    IRepository<History> historyRepository,
    IHistoryQueries historyQueries) : IRequestHandler<ChangeHistoryActionTypeCommand, Either<BaseException, History>>
{
    public async Task<Either<BaseException, History>> Handle(ChangeHistoryActionTypeCommand request, CancellationToken cancellationToken)
    {
        var history = await historyQueries.GetByIdAsync(request.Id, cancellationToken);

        return await history.MatchAsync(
            h => UpdateEntity(request, h, cancellationToken),
            () => new HistoryNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, History>> UpdateEntity(
        ChangeHistoryActionTypeCommand request,
        History history,
        CancellationToken cancellationToken)
    {
        try
        {
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
}