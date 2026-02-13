using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IContainerTypeProductTypeRepository
{
    Task<ContainerTypeProductType> DeleteRangeAsync(
        IEnumerable<ContainerTypeProductType> entities,
        CancellationToken cancellationToken);
    
    Task CreateRangeAsync(IReadOnlyList<ContainerTypeProductType> entities, CancellationToken ct);
}