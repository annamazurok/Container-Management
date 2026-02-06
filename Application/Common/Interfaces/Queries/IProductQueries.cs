using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IProductQueries : IBaseQuery<Product>
{
    Task<IReadOnlyList<Product>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetExpiredProductsAsync(DateTime currentDate, CancellationToken cancellationToken);
}