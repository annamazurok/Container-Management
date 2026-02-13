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
    public required IReadOnlyList<int> ProductTypeIds { get; init; }
}

public class UpdateContainerTypeCommandHandler(
    IRepository<ContainerType> containerTypeRepository,
    IContainerTypeQueries containerTypeQueries,
    IUnitQueries unitQueries,
    IProductTypeQueries productTypeQueries,
    IContainerTypeProductTypeRepository containerTypeProductTypeRepository)
    : IRequestHandler<UpdateContainerTypeCommand, Either<BaseException, ContainerType>>
{
    public async Task<Either<BaseException, ContainerType>> Handle(
        UpdateContainerTypeCommand request,
        CancellationToken cancellationToken)
    {
        var containerType = await containerTypeQueries.GetByIdAsync(request.Id, cancellationToken);

        return await containerType.MatchAsync(
            ct => CheckDuplicates(ct.Id, request.Name, cancellationToken)
                .BindAsync(_ => CheckUnit(request.UnitId, cancellationToken)
                    .BindAsync(_ => CheckProductTypes(request.ProductTypeIds, cancellationToken)
                        .BindAsync(_ => UpdateEntity(request, ct, cancellationToken)))),
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
                request.UnitId);

            var existingRelations = await containerTypeProductTypeRepository
                .GetByContainerTypeAsync(containerType.Id, cancellationToken);

            var existingIds = existingRelations.Select(x => x.ProductTypeId).ToHashSet();

            var toAdd = request.ProductTypeIds
                .Where(id => !existingIds.Contains(id))
                .Select(id => ContainerTypeProductType.New(containerType.Id, id, 1))
                .ToList();

            var toRemove = existingRelations
                .Where(r => !request.ProductTypeIds.Contains(r.ProductTypeId))
                .ToList();

            if (toRemove.Any())
                await containerTypeProductTypeRepository.DeleteRangeAsync(toRemove, cancellationToken);

            if (toAdd.Any())
                await containerTypeProductTypeRepository.CreateRangeAsync(toAdd, cancellationToken);

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

    private async Task<Either<BaseException, Unit>> CheckProductTypes(
        IReadOnlyList<int> productTypeIds, CancellationToken cancellationToken)
    {
        foreach (var productTypeId in productTypeIds)
        {
            var productType = await productTypeQueries.GetByIdAsync(productTypeId, cancellationToken);
            if (productType.IsNone)
                return new ProductTypeNotFoundException(productTypeId);
        }

        return Unit.Default;
    }
    
    private async Task<Either<BaseException, Unit>> CheckDuplicates(
        int currentContainerTypeId,
        string name,
        CancellationToken cancellationToken)
    {
        var containerType = await containerTypeQueries.GetByNameAsync(name, cancellationToken);

        return containerType.Match<Either<BaseException, Unit>>(
            ct => ct.Id == currentContainerTypeId
                ? Unit.Default
                : new ContainerTypeAlreadyExistException(ct.Id),
            () => Unit.Default);
    }

}
