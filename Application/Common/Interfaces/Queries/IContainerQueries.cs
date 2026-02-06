using Domain;
using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IContainerQueries : IBaseQuery<Container>
{
    Task<IReadOnlyList<Container>> GetByStatusAsync(Status status, CancellationToken cancellationToken);
    Task<IReadOnlyList<Container>> GetByContainerTypeAsync(int containerTypeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Container>> GetByProductAsync(int productId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Container>> GetActiveContainersAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Container>> GetExpiringContainersAsync(DateTime beforeDate, CancellationToken cancellationToken);
    Task<Option<Container>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Container>> GetContainersByProductTypeAsync(int productTypeId, CancellationToken cancellationToken);
} 