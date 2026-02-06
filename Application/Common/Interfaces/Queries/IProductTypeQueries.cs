using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IProductTypeQueries : IBaseQuery<ProductType>
{
    Task<ProductType?> GetByTitleAsync(string title, CancellationToken cancellationToken);
}