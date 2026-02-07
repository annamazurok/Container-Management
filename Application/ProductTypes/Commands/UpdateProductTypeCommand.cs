using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.ProductTypes.Commands;

public class UpdateProductTypeCommand : IRequest<Either<BaseException, ProductType>>
{
    public required int Id { get; init; }
    public required string Title { get; init; }
}

public class UpdateProductTypeCommandHandler(
    IRepository<ProductType> productTypeRepository,
    IProductTypeQueries productTypeQueries)
    : IRequestHandler<UpdateProductTypeCommand, Either<BaseException, ProductType>>
{
    public async Task<Either<BaseException, ProductType>> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var productType = await productTypeQueries.GetByIdAsync(request.Id, cancellationToken);

        return await productType.MatchAsync(
            pt => CheckDuplicates(pt.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, pt, cancellationToken)),
            () => new ProductTypeNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, ProductType>> UpdateEntity(
        UpdateProductTypeCommand request,
        ProductType productType,
        CancellationToken cancellationToken)
    {
        try
        {
            productType.UpdateTitle(request.Title, 1); // TODO: Replace with actual userId from ICurrentUserService

            return await productTypeRepository.UpdateAsync(productType, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledProductTypeException(productType.Id, ex);
        }
    }

    private async Task<Either<BaseException, Unit>> CheckDuplicates(
        int currentId,
        string title,
        CancellationToken cancellationToken)
    {
        var productType = await productTypeQueries.GetByTitleAsync(title, cancellationToken);

        return productType.Match<Either<BaseException, Unit>>(
            pt => pt.Id.Equals(currentId) ? Unit.Default : new ProductTypeAlreadyExistsException(pt.Id),
            () => Unit.Default);
    }
}