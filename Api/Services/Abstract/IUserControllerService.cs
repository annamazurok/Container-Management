using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract;

public interface IUserControllerService : IControllerService<UserDto>
{
    Task<Option<UserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserDto>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken);
}