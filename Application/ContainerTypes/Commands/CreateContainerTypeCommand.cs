using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.ContainerTypes.Commands;

public class CreateContainerTypeCommand : IRequest<Either<BaseException, ContainerType>>
{
    public required string Name { get; init; }
    public required int Volume { get; init; }
    public required int UnitId { get; init; }
}

public class CreateContainerTypeCommandHandler(
    IRepository<ContainerType> containerTypeRepository,
    IContainerTypeQueries containerTypeQueries,
    IUnitQueries unitQueries) : IRequestHandler<CreateContainerTypeCommand, Either<BaseException, ContainerType>>
{
    public async Task<Either<BaseException, ContainerType>> Handle(CreateContainerTypeCommand request, CancellationToken cancellationToken)
    {
        var existingContainerType = await containerTypeQueries.GetByNameAsync(request.Name, cancellationToken);

        return await existingContainerType.MatchAsync(
            ct => new ContainerTypeAlreadyExistException(ct.Id),
            () => CheckUnit(request.UnitId, cancellationToken)
                .BindAsync(_ => CreateEntity(request, cancellationToken)));
    }

    private async Task<Either<BaseException, Unit>> CheckUnit(
        int unitId, CancellationToken cancellationToken)
    {
        var unit = await unitQueries.GetByIdAsync(unitId, cancellationToken);

        return unit.IsNone
            ? new UnitNotFoundException(unitId)
            : Unit.Default;
    }

    private async Task<Either<BaseException, ContainerType>> CreateEntity(
        CreateContainerTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var containerType = await containerTypeRepository.CreateAsync(
                ContainerType.New(
                    0,
                    request.Name,
                    request.Volume,
                    request.UnitId), // TODO: Replace with actual userId from ICurrentUserService
                cancellationToken);

            return containerType;
        }
        catch (Exception ex)
        {
            return new UnhandledContainerTypeException(0, ex);
        }
    }
}