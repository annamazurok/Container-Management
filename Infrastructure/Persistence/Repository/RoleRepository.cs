using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class RoleRepository : BaseRepository<Role>, IRepository<Role>, IRoleQueries
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context, ApplicationSettings settings) : base(context, settings)
    {
        _context = context;
    }

    public async Task<Role> CreateAsync(Role entity, CancellationToken cancellationToken)
    {
        await _context.Roles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken)
    {
        _context.Roles.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken)
    {
        _context.Roles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Role> DeleteRangeAsync(IEnumerable<Role> entities, CancellationToken cancellationToken)
    {
        _context.Roles.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }
}