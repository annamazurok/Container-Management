using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class UserRepository : BaseRepository<User>, IRepository<User>, IUserQueries
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context, ApplicationSettings settings) : base(context, settings)
    {
        _context = context;
    }
    
    public new async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .Include(x => x.Role)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<User> UpdateAsync(User entity, CancellationToken cancellationToken)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<User> DeleteAsync(User entity, CancellationToken cancellationToken)
    {
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<User> DeleteRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken)
    {
        _context.Users.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
        return entities.FirstOrDefault(); 
    }

    public async Task<IReadOnlyList<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.RoleId == roleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .Include(x => x.Role)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

        return entity ?? Option<User>.None;
    }

    public async Task<Option<User>> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .Include(x => x.Role)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.GoogleId == googleId, cancellationToken);

        return entity ?? Option<User>.None;
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.CountAsync(cancellationToken);
    }
}