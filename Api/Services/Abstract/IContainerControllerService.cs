using Api.Dtos;
using Domain;
using LanguageExt;

namespace Api.Services.Abstract;

public interface IContainerControllerService : IControllerService<ContainerDto>
{
    Task<Option<ContainerDto>> GetByNameAsync(string containerName, CancellationToken cancellationToken);
    Task<Option<ContainerDto>> GetByCodeAsync(string code, CancellationToken cancellationToken);
    Task<IReadOnlyList<ContainerDto>> GetByTypeAsync(int typeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ContainerDto>> GetByProductAsync(int productId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ContainerDto>> GetByStatusAsync(Status status, CancellationToken cancellationToken);
}