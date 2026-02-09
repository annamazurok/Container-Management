using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation;

public class RoleControllerService(IRoleQueries roleQueries) : IRoleControllerService
{
    public async Task<IReadOnlyList<RoleDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await roleQueries.GetAllAsync(cancellationToken);

        return result.Select(RoleDto.FromDomainModel).ToList();
    }

    public async Task<Option<RoleDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await roleQueries.GetByIdAsync(id, cancellationToken);
        return result.Match(
            r => RoleDto.FromDomainModel(r),
            () => Option<RoleDto>.None);
    }
}