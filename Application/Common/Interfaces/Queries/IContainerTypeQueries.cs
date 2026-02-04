using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IContainerTypeQueries
{
    Task<IReadOnlyList<ContainerType>> GetAllAsync(CancellationToken cancellationToken);
    Task<Option<ContainerType>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Option<ContainerType>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<ContainerType>> GetByUnitAsync(int unitId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ContainerType>> GetByVolumeRangeAsync(int minVolume, int maxVolume, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductType>> GetCompatibleProductTypesAsync(int containerTypeId, CancellationToken cancellationToken);
}