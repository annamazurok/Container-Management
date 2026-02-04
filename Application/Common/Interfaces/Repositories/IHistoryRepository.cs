using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IHistoryRepository
{
    Task<History> AddAsync(History entity, CancellationToken cancellationToken);
    Task<History> UpdateAsync(History entity, CancellationToken cancellationToken);
    Task<History> DeleteAsync(History entity, CancellationToken cancellationToken);
}