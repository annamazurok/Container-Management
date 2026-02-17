using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Products.Commands;

public class CreateProductCommand : IRequest<Either<BaseException, Product>>
{
    public required int TypeId { get; init; }
    public required string Name { get; init; }
    public required DateTime Produced { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public string? Description { get; init; }
}

public class CreateProductCommandHandler(
    IRepository<Product> productRepository, 
    IProductTypeQueries productTypeQueries)
    : IRequestHandler<CreateProductCommand, Either<BaseException, Product>>
{
    public async Task<Either<BaseException, Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return await CheckDependencies(request, cancellationToken)
            .BindAsync(_ => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, Unit>> CheckDependencies(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productType = await productTypeQueries.GetByIdAsync(request.TypeId, cancellationToken);

        if (productType.IsNone)
            return new ProductTypeNotFoundException(request.TypeId);

        return Unit.Default;
    }

    private async Task<Either<BaseException, Product>> CreateEntity(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.CreateAsync(
                Product.New(
                    request.TypeId,
                    request.Name,
                    request.Produced,
                    request.ExpirationDate,
                    request.Description,
                    1), // TODO: Replace with actual userId from ICurrentUserService
                cancellationToken);

            return product;
        }
        catch (Exception ex)
        {
            return new UnhandledProductException(0, ex);
        }
    }
}