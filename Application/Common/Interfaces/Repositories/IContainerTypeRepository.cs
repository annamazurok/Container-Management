using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IContainerTypeRepository
{
    Task<ContainerType> AddAsync(ContainerType entity, CancellationToken cancellationToken);
    Task<ContainerType> UpdateAsync(ContainerType entity, CancellationToken cancellationToken);
    Task<ContainerType> DeleteAsync(ContainerType entity, CancellationToken cancellationToken);
}