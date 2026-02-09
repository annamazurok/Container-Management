using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Histories.Commands;

public class DeleteHistoryCommand : IRequest<Either<BaseException, History>>
{
    public int Id { get; init; }
}

public class DeleteHistoryCommandHandler(
    IRepository<History> historyRepository,
    IHistoryQueries historyQueries) : IRequestHandler<DeleteHistoryCommand, Either<BaseException, History>>
{
    public async Task<Either<BaseException, History>> Handle(DeleteHistoryCommand request, CancellationToken cancellationToken)
    {
        var history = await historyQueries.GetByIdAsync(request.Id, cancellationToken);

        return await history.MatchAsync(
            h => DeleteEntity(h, cancellationToken),
            () => new HistoryNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, History>> DeleteEntity(
        History history,
        CancellationToken cancellationToken)
    {
        try
        {
            await historyRepository.DeleteAsync(history, cancellationToken);

            return history;
        }
        catch (Exception ex)
        {
            return new UnhandledHistoryException(history.Id, ex);
        }
    }
}