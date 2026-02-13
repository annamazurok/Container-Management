using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IContainerTypeProductTypeQuery
{
    Task<bool> ExistsAsync(
        int containerTypeId,
        int productTypeId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ContainerTypeProductType>> GetByContainerTypeAsync(
        int containerTypeId,
        CancellationToken cancellationToken);
}