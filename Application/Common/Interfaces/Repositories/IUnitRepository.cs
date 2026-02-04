using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IUnitRepository
{
    Task<Unit> AddAsync(Unit entity, CancellationToken cancellationToken);
    Task<Unit> UpdateAsync(Unit entity, CancellationToken cancellationToken);
    Task<Unit> DeleteAsync(Unit entity, CancellationToken cancellationToken);
}