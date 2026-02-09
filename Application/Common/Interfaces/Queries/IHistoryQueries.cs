using Domain;
using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IHistoryQueries : IBaseQuery<History>
{
    Task<IReadOnlyList<History>> GetByContainerAsync(int containerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<History>> GetByProductAsync(int productId, CancellationToken cancellationToken);
    Task<IReadOnlyList<History>> GetByActionTypeAsync(Status actionType, CancellationToken cancellationToken);
    Task<IReadOnlyList<History>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
    Task<IReadOnlyList<History>> GetByUserAsync(int userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<History>> GetRecentHistoryAsync(int count, CancellationToken cancellationToken);
    Task<IReadOnlyList<History>> GetContainerHistoryByDateAsync(int containerId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
}