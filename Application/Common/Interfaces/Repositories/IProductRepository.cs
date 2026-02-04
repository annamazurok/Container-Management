using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product> AddAsync(Product entity, CancellationToken cancellationToken);
    Task<Product> UpdateAsync(Product entity, CancellationToken cancellationToken);
    Task<Product> DeleteAsync(Product entity, CancellationToken cancellationToken);
}