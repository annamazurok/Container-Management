using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Units.Commands;

public class DeleteUnitCommand : IRequest<Either<BaseException, Domain.Entities.Unit>>
{
    public int Id { get; init; }
}

public class DeleteUnitCommandHandler(
    IRepository<Domain.Entities.Unit> unitRepository,
    IUnitQueries unitQueries,
    IContainerQueries containerQueries,
    IContainerTypeQueries containerTypeQueries,
    IHistoryQueries historyQueries) : IRequestHandler<DeleteUnitCommand, Either<BaseException, Domain.Entities.Unit>>
{
    public async Task<Either<BaseException, Domain.Entities.Unit>> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitQueries.GetByIdAsync(request.Id, cancellationToken);

        return await unit.MatchAsync(
            u => CheckDependencies(u.Id, cancellationToken)
                .BindAsync(_ => DeleteEntity(u, cancellationToken)),
            () => new UnitNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Domain.Entities.Unit>> DeleteEntity(
        Domain.Entities.Unit unit,
        CancellationToken cancellationToken)
    {
        try
        {
            await unitRepository.DeleteAsync(unit, cancellationToken);

            return unit;
        }
        catch (Exception ex)
        {
            return new UnhandledUnitException(unit.Id, ex);
        }
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        int unitId,
        CancellationToken cancellationToken)
    {
        var containers = await containerQueries.GetByIdAsync(unitId, cancellationToken);
        if (containers.Any())
            return new UnitHasContainersException(unitId);

        var containerTypes = await containerTypeQueries.GetByUnitAsync(unitId, cancellationToken);
        if (containerTypes.Any())
            return new UnitHasContainerTypesException(unitId);

        var histories = await historyQueries.GetByIdAsync(unitId, cancellationToken);
        if (histories.Any())
            return new UnitHasHistoriesException(unitId);

        return LanguageExt.Unit.Default;
    }
}