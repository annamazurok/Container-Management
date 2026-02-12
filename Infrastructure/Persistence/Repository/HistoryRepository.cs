using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class HistoryRepository : IRepository<History>, IHistoryQueries
{
    private readonly ApplicationDbContext _context;

    public HistoryRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        
        _context = context;
    }

    public async Task<History> CreateAsync(History entity, CancellationToken cancellationToken)
    {
        await _context.Histories.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<History> UpdateAsync(History entity, CancellationToken cancellationToken)
    {
        _context.Histories.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<History> DeleteAsync(History entity, CancellationToken cancellationToken)
    {
        _context.Histories.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<History> DeleteRangeAsync(IEnumerable<History> entities, CancellationToken cancellationToken)
    {
        _context.Histories.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }

    public async Task<IReadOnlyList<History>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Histories.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Option<History>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Histories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<History>.None;
    }

    public async Task<Option<History>> GetByIdAsync(int? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
            return Option<History>.None;

        return await GetByIdAsync(id.Value, cancellationToken);
    }

    public async Task<IReadOnlyList<History>> GetByContainerAsync(int containerId, CancellationToken cancellationToken)
    {
        return await _context.Histories
            .AsNoTracking()
            .Where(h => h.ContainerId == containerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<History>> GetByProductAsync(int productId, CancellationToken cancellationToken)
    {
        return await _context.Histories
            .AsNoTracking()
            .Where(h => h.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<History>> GetByActionTypeAsync(Status actionType, CancellationToken cancellationToken)
    {
        return await _context.Histories
            .AsNoTracking()
            .Where(h => h.ActionType == actionType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<History>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
    {
        return await _context.Histories
            .AsNoTracking()
            .Where(h => h.FromDate >= fromDate && h.FromDate <= toDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<History>> GetByUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Histories
            .AsNoTracking()
            .Where(h => h.CreatedBy == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<History>> GetRecentHistoryAsync(int count, CancellationToken cancellationToken)
    {
        return await _context.Histories
            .AsNoTracking()
            .OrderByDescending(h => h.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<History>> GetContainerHistoryByDateAsync(int containerId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
    {
        return await _context.Histories
            .AsNoTracking()
            .Where(h => h.ContainerId == containerId && h.FromDate >= fromDate && h.FromDate <= toDate)
            .ToListAsync(cancellationToken);
    }
}