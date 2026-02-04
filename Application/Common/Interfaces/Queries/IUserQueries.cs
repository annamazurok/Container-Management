using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IUserQueries
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken);
}