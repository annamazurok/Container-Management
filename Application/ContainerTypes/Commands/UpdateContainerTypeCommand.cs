using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.ContainerTypes.Commands;

public class UpdateContainerTypeCommand : IRequest<Either<BaseException, ContainerType>>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Volume { get; init; }
    public required int UnitId { get; init; }
}

public class UpdateContainerTypeCommandHandler(
    IRepository<ContainerType> containerTypeRepository,
    IContainerTypeQueries containerTypeQueries,
    IUnitQueries unitQueries) : IRequestHandler<UpdateContainerTypeCommand, Either<BaseException, ContainerType>>
{
    public async Task<Either<BaseException, ContainerType>> Handle(UpdateContainerTypeCommand request, CancellationToken cancellationToken)
    {
        var containerType = await containerTypeQueries.GetByIdAsync(request.Id, cancellationToken);

        return await containerType.MatchAsync(
            ct => CheckDuplicates(ct.Id, request.Name, cancellationToken)
                .BindAsync(_ => CheckUnit(request.UnitId, cancellationToken)
                    .BindAsync(_ => UpdateEntity(request, ct, cancellationToken))),
            () => new ContainerTypeNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, ContainerType>> UpdateEntity(
        UpdateContainerTypeCommand request,
        ContainerType containerType,
        CancellationToken cancellationToken)
    {
        try
        {
            containerType.UpdateDetails(
                request.Name,
                request.Volume,
                request.UnitId); // TODO: Replace with actual userId from ICurrentUserService

            return await containerTypeRepository.UpdateAsync(containerType, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledContainerTypeException(containerType.Id, ex);
        }
    }

    private async Task<Either<BaseException, Unit>> CheckUnit(
        int unitId, CancellationToken cancellationToken)
    {
        var unit = await unitQueries.GetByIdAsync(unitId, cancellationToken);

        return unit.IsNone
            ? new UnitNotFoundException(unitId)
            : Unit.Default;
    }

    private async Task<Either<BaseException, Unit>> CheckDuplicates(
        int currentContainerTypeId,
        string name,
        CancellationToken cancellationToken)
    {
        var containerType = await containerTypeQueries.GetByNameAsync(name, cancellationToken);

        return containerType.Match<Either<BaseException, Unit>>(
            ct => ct.Id.Equals(currentContainerTypeId) ? Unit.Default : new ContainerTypeAlreadyExistException(ct.Id),
            () => Unit.Default);
    }
}