using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IProductQueries
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetExpiredProductsAsync(DateTime currentDate, CancellationToken cancellationToken);
}