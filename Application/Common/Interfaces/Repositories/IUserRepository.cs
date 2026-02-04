using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> AddAsync(User entity, CancellationToken cancellationToken);
    Task<User> UpdateAsync(User entity, CancellationToken cancellationToken);
    Task<User> DeleteAsync(User entity, CancellationToken cancellationToken);
}