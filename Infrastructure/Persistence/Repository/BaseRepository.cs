using Application.Common.Interfaces.Queries;
using Application.Settings;
using Domain.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public abstract class BaseRepository<T> : IBaseQuery<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _context;

    protected BaseRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        _context = context;
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<Option<T>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<T>.None;
    }

    public virtual async Task<Option<T>> GetByIdAsync(int? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
            return Option<T>.None;

        return await GetByIdAsync(id.Value, cancellationToken);
    }
}
