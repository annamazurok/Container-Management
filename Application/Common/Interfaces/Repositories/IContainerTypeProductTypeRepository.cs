using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IContainerTypeProductTypeRepository
{
    Task<ContainerTypeProductType> CreateAsync(
        ContainerTypeProductType entity,
        CancellationToken cancellationToken);

    Task<ContainerTypeProductType> DeleteAsync(
        ContainerTypeProductType entity,
        CancellationToken cancellationToken);

    Task<ContainerTypeProductType> DeleteRangeAsync(
        IEnumerable<ContainerTypeProductType> entities,
        CancellationToken cancellationToken);

    Task<bool> ExistsAsync(
        int containerTypeId,
        int productTypeId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ContainerTypeProductType>> GetByContainerTypeAsync(
        int containerTypeId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ContainerTypeProductType>> GetByProductTypeAsync(
        int productTypeId,
        CancellationToken cancellationToken);
    
    Task CreateRangeAsync(IReadOnlyList<ContainerTypeProductType> entities, CancellationToken ct);
}