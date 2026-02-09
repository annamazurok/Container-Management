using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation;

public class UserControllerService(IUserQueries userQueries) : IUserControllerService
{
    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await userQueries.GetAllAsync(cancellationToken);

        return result.Select(UserDto.FromDomainModel).ToList();
    }

    public async Task<Option<UserDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await userQueries.GetByIdAsync(id, cancellationToken);
        return result.Match(
            u => UserDto.FromDomainModel(u),
            () => Option<UserDto>.None);
    }

    public async Task<Option<UserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var result = await userQueries.GetByEmailAsync(email, cancellationToken);
        return result.Match(
            u => UserDto.FromDomainModel(u),
            () => Option<UserDto>.None);
    }

    public async Task<IReadOnlyList<UserDto>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken)
    {
        var result = await userQueries.GetByRoleIdAsync(roleId, cancellationToken);

        return result.Select(UserDto.FromDomainModel).ToList();
    }
}