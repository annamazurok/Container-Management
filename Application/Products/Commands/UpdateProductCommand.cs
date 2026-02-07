using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Products.Commands;

public class UpdateProductCommand : IRequest<Either<BaseException, Product>>
{
    public required int Id { get; init; }
    public required int TypeId { get; init; }
    public required DateTime Produced { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public string? Description { get; init; }
}

public class UpdateProductCommandHandler(
    IRepository<Product> productRepository,
    IProductQueries productQueries,
    IProductTypeQueries productTypeQueries)
    : IRequestHandler<UpdateProductCommand, Either<BaseException, Product>>
{
    public async Task<Either<BaseException, Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productQueries.GetByIdAsync(request.Id, cancellationToken);

        return await product.MatchAsync(
            p => CheckDependencies(request.TypeId, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, p, cancellationToken)),
            () => new ProductNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Product>> UpdateEntity(
        UpdateProductCommand request,
        Product product,
        CancellationToken cancellationToken)
    {
        try
        {
            product.UpdateDetails(
                request.Produced,
                request.ExpirationDate,
                request.Description,
                1); // TODO: Replace with actual userId from ICurrentUserService

            product.ChangeType(
                request.TypeId,
                1); // TODO: Replace with actual userId from ICurrentUserService

            return await productRepository.UpdateAsync(product, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledProductException(product.Id, ex);
        }
    }

    private async Task<Either<BaseException, Unit>> CheckDependencies(
        int typeId,
        CancellationToken cancellationToken)
    {
        var type = await productTypeQueries.GetByIdAsync(typeId, cancellationToken);

        return type.IsNone
            ? new ProductTypeNotFoundException(typeId)
            : Unit.Default;
    }
}