using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.ProductTypes.Commands;

public class DeleteProductTypeCommand : IRequest<Either<BaseException, ProductType>>
{
    public int Id { get; init; }
}

public class DeleteProductTypeCommandHandler(
    IRepository<ProductType> productTypeRepository,
    IProductTypeQueries productTypeQueries,
    IProductQueries productQueries)
    : IRequestHandler<DeleteProductTypeCommand, Either<BaseException, ProductType>>
{
    public async Task<Either<BaseException, ProductType>> Handle(
        DeleteProductTypeCommand request,
        CancellationToken cancellationToken)
    {
        var productType = await productTypeQueries.GetByIdAsync(request.Id, cancellationToken);

        return await productType.MatchAsync(
            pt => CheckDependencies(pt.Id, cancellationToken)
                .BindAsync(_ => DeleteEntity(pt, cancellationToken)),
            () => new ProductTypeNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, ProductType>> DeleteEntity(
        ProductType productType,
        CancellationToken cancellationToken)
    {
        try
        {
            await productTypeRepository.DeleteAsync(productType, cancellationToken);
            return productType;
        }
        catch (Exception ex)
        {
            return new UnhandledProductTypeException(productType.Id, ex);
        }
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        int productTypeId,
        CancellationToken cancellationToken)
    {
        var products = await productQueries.GetByTypeIdAsync(productTypeId, cancellationToken);

        return products.Any()
            ? new ProductTypeHasProductsException(productTypeId)
            : LanguageExt.Unit.Default;
    }
}