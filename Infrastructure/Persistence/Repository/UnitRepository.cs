using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Unit = Domain.Entities.Unit;

namespace Infrastructure.Persistence.Repository;

public class UnitRepository : BaseRepository<Unit>, IRepository<Unit>, IUnitQueries
{
    private readonly ApplicationDbContext _context;

    public UnitRepository(ApplicationDbContext context, ApplicationSettings settings) : base(context, settings)
    {
        _context = context;
    }

    public async Task<Unit> CreateAsync(Unit entity, CancellationToken cancellationToken)
    {
        await _context.Units.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Unit> UpdateAsync(Unit entity, CancellationToken cancellationToken)
    {
        _context.Units.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Unit> DeleteAsync(Unit entity, CancellationToken cancellationToken)
    {
        _context.Units.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Unit> DeleteRangeAsync(IEnumerable<Unit> entities, CancellationToken cancellationToken)
    {
        _context.Units.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }

    public async Task<Option<Unit>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var entity = await _context.Units
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Title == title, cancellationToken);

        return entity ?? Option<Unit>.None;
    }

    public async Task<IReadOnlyList<Unit>> GetByUnitTypeAsync(UnitType unitType, CancellationToken cancellationToken)
    {
        return (IReadOnlyList<Unit>)await _context.Units
            .AsNoTracking()
            .Where(u => u.UnitType == unitType)
            .ToListAsync(cancellationToken);
    }
}