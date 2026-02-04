using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IProductTypeQueries
{
    Task<IReadOnlyList<ProductType>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProductType?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ProductType?> GetByTitleAsync(string title, CancellationToken cancellationToken);
}