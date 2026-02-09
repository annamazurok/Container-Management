using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation;

public class ProductControllerService(IProductQueries productQueries) : IProductControllerService
{
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await productQueries.GetAllAsync(cancellationToken);

        return result.Select(ProductDto.FromDomainModel).ToList();
    }

    public async Task<Option<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await productQueries.GetByIdAsync(id, cancellationToken);
        return result.Match(
            p => ProductDto.FromDomainModel(p),
            () => Option<ProductDto>.None);
    }

    public async Task<IReadOnlyList<ProductDto>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken)
    {
        var result = await productQueries.GetByTypeIdAsync(typeId, cancellationToken);

        return result.Select(ProductDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetExpiredProductsAsync(DateTime currentDate, CancellationToken cancellationToken)
    {
        var result = await productQueries.GetExpiredProductsAsync(currentDate, cancellationToken);

        return result.Select(ProductDto.FromDomainModel).ToList();
    }
}