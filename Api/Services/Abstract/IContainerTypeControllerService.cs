using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract;

public interface IContainerTypeControllerService : IControllerService<ContainerTypeDto>
{
    Task<Option<ContainerTypeDto>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<ContainerTypeDto>> GetByVolumeRangeAsync(int minVolume, int maxVolume, CancellationToken cancellationToken);
    Task<IReadOnlyList<ContainerTypeDto>> GetByUnitIdAsync(int unitId, CancellationToken cancellationToken);
}