using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Containers.Commands;

public class DeleteContainerCommand : IRequest<Either<BaseException, Container>>
{
    public int Id { get; init; }
}

public class DeleteContainerCommandHandler(
    IRepository<Container> containerRepository,
    IContainerQueries containerQueries,
    IHistoryQueries historyQueries,
    IRepository<History> historyRepository) : IRequestHandler<DeleteContainerCommand, Either<BaseException, Container>>
{
    public async Task<Either<BaseException, Container>> Handle(DeleteContainerCommand request, CancellationToken cancellationToken)
    {
        var container = await containerQueries.GetByIdAsync(request.Id, cancellationToken);

        return await container.MatchAsync(
            c => DeleteEntity(c, cancellationToken),
            () => new ContainerNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Container>> DeleteEntity(
        Container container,
        CancellationToken cancellationToken)
    {
        try
        {
            var histories = await historyQueries.GetByContainerAsync(container.Id, cancellationToken);

            if (histories.Any())
                await historyRepository.DeleteRangeAsync(histories, cancellationToken);

            await containerRepository.DeleteAsync(container, cancellationToken);

            return container;
        }
        catch (Exception ex)
        {
            return new UnhandledContainerException(container.Id, ex);
        }
    }
}