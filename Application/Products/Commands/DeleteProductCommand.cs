using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Products.Commands;

public class DeleteProductCommand : IRequest<Either<BaseException, Product>>
{
    public int Id { get; init; }
}

public class DeleteProductCommandHandler(
    IRepository<Product> productRepository,
    IProductQueries productQueries)
    : IRequestHandler<DeleteProductCommand, Either<BaseException, Product>>
{
    public async Task<Either<BaseException, Product>> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await productQueries.GetByIdAsync(request.Id, cancellationToken);

        return await product.MatchAsync(
            p => DeleteEntity(p, cancellationToken),
            () => new ProductNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Product>> DeleteEntity(
        Product product,
        CancellationToken cancellationToken)
    {
        try
        {
            await productRepository.DeleteAsync(product, cancellationToken);
            return product;
        }
        catch (Exception ex)
        {
            return new UnhandledProductException(product.Id, ex);
        }
    }
}