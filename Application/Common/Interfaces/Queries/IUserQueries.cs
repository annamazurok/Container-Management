using Domain.Entities;

namespace Application.Common.Interfaces.Queries;

public interface IUserQueries : IBaseQuery<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken);
}