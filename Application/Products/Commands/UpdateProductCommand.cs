using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Products.Commands;

public class UpdateProductCommand : IRequest<Either<BaseException, Product>>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int TypeId { get; init; }
    public required DateTime Produced { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public string? Description { get; init; }
}

public class UpdateProductCommandHandler(
    IRepository<Product> productRepository,
    IProductQueries productQueries,
    IProductTypeQueries productTypeQueries,
    ICurrentUserService currentUserService)
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
            var userId = currentUserService.UserId
            ?? throw new UnauthorizedException("User not authenticated");

            product.UpdateDetails(
                request.TypeId,
                request.Name,
                request.Produced,
                request.ExpirationDate,
                request.Description,
                userId); 

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