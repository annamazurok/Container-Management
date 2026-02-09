using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation;

public class ProductTypeControllerService(IProductTypeQueries productTypeQueries) : IProductTypeControllerService
{
    public async Task<IReadOnlyList<ProductTypeDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await productTypeQueries.GetAllAsync(cancellationToken);

        return result.Select(ProductTypeDto.FromDomainModel).ToList();
    }

    public async Task<Option<ProductTypeDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await productTypeQueries.GetByIdAsync(id, cancellationToken);
        return result.Match(
            pt => ProductTypeDto.FromDomainModel(pt),
            () => Option<ProductTypeDto>.None);
    }

    public async Task<Option<ProductTypeDto>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var result = await productTypeQueries.GetByTitleAsync(title, cancellationToken);
        return result.Match(
            pt => ProductTypeDto.FromDomainModel(pt),
            () => Option<ProductTypeDto>.None
        );
    }
}