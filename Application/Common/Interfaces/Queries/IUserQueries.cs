using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IUserQueries : IBaseQuery<User>
{
    Task<Option<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Option<User>> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
}