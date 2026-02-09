using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation;

public class ContainerTypeControllerService(
    IContainerTypeQueries containerTypeQueries) : IContainerTypeControllerService
{
    public async Task<IReadOnlyList<ContainerTypeDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await containerTypeQueries.GetAllAsync(cancellationToken);
        
        return result.Select(ContainerTypeDto.FromDomainModel).ToList();
    }

    public async Task<Option<ContainerTypeDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var  result = await containerTypeQueries.GetByIdAsync(id, cancellationToken);

        return result.Match(
            ct => ContainerTypeDto.FromDomainModel(ct),
            () => Option<ContainerTypeDto>.None);
    }

    public async Task<Option<ContainerTypeDto>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var  result = await containerTypeQueries.GetByNameAsync(name, cancellationToken);

        return result.Match(
            ct => ContainerTypeDto.FromDomainModel(ct),
            () => Option<ContainerTypeDto>.None);
    }

    public async Task<IReadOnlyList<ContainerTypeDto>> GetByVolumeRangeAsync(int minVolume, int maxVolume, CancellationToken cancellationToken)
    {
        var  result = await containerTypeQueries.GetByVolumeRangeAsync(minVolume, maxVolume, cancellationToken);

        return result.Select(ContainerTypeDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<ContainerTypeDto>> GetByUnitIdAsync(int unitId, CancellationToken cancellationToken)
    {
        var  result = await containerTypeQueries.GetByUnitAsync(unitId, cancellationToken);

        return result.Select(ContainerTypeDto.FromDomainModel).ToList();
    }
}