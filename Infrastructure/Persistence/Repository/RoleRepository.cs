using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class RoleRepository : IRepository<Role>, IRoleQueries
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        
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

    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Roles.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Option<Role>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<Role>.None;
    }

    public async Task<Option<Role>> GetByIdAsync(int? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
            return Option<Role>.None;

        return await GetByIdAsync(id.Value, cancellationToken);
    }
}