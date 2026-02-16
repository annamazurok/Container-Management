using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
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
    public required IReadOnlyList<int> ProductTypeIds { get; init; }
}

public class CreateContainerTypeCommandHandler(
    IRepository<ContainerType> containerTypeRepository,
    IContainerTypeQueries containerTypeQueries,
    IUnitQueries unitQueries,
    IProductTypeQueries productTypeQueries, 
    ICurrentUserService currentUserService) : IRequestHandler<CreateContainerTypeCommand, Either<BaseException, ContainerType>>
{
    public async Task<Either<BaseException, ContainerType>> Handle(CreateContainerTypeCommand request, CancellationToken cancellationToken)
    {
        var existingContainerType = await containerTypeQueries.GetByNameAsync(request.Name, cancellationToken);

        return await existingContainerType.MatchAsync(
            ct => new ContainerTypeAlreadyExistException(ct.Id),
            () => CheckUnit(request.UnitId, cancellationToken)
                .BindAsync(_ => CheckProductTypes(request.ProductTypeIds, cancellationToken)
                    .BindAsync(_ => CreateEntity(request, cancellationToken))));
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

    private async Task<Either<BaseException, ContainerType>> CreateEntity(
        CreateContainerTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUserService.UserId
            ?? throw new UnauthorizedException("User not authenticated");

            var productTypeAssignments = request.ProductTypeIds
                .Select(productTypeId => ContainerTypeProductType.New(
                    0, 
                    productTypeId,
                    userId)) 
                .ToArray();

            var containerType = await containerTypeRepository.CreateAsync(
                ContainerType.New(
                    request.Name,
                    request.Volume,
                    request.UnitId,
                    productTypeAssignments,
                    userId),
                cancellationToken);

            return containerType;
        }
        catch (Exception ex)
        {
            return new UnhandledContainerTypeException(0, ex);
        }
    }
}