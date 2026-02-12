using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ContainerTypeRepository : IRepository<ContainerType>, IContainerTypeQueries
{
    private readonly ApplicationDbContext _context;

    public ContainerTypeRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        
        _context = context;
    }

    public async Task<ContainerType> CreateAsync(ContainerType entity, CancellationToken cancellationToken)
    {
        await _context.ContainerTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ContainerType> UpdateAsync(ContainerType entity, CancellationToken cancellationToken)
    {
        _context.ContainerTypes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ContainerType> DeleteAsync(ContainerType entity, CancellationToken cancellationToken)
    {
        _context.ContainerTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ContainerType> DeleteRangeAsync(IEnumerable<ContainerType> entities, CancellationToken cancellationToken)
    {
        _context.ContainerTypes.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }

    public async Task<IReadOnlyList<ContainerType>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.ContainerTypes.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Option<ContainerType>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.ContainerTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<ContainerType>.None;
    }

    public async Task<Option<ContainerType>> GetByIdAsync(int? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
            return Option<ContainerType>.None;

        return await GetByIdAsync(id.Value, cancellationToken);
    }

    public async Task<Option<ContainerType>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.ContainerTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Name == name, cancellationToken);

        return entity ?? Option<ContainerType>.None;
    }

    public async Task<IReadOnlyList<ContainerType>> GetByUnitAsync(int unitId, CancellationToken cancellationToken)
    {
        return await _context.ContainerTypes
            .AsNoTracking()
            .Where(ct => ct.UnitId == unitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ContainerType>> GetByVolumeRangeAsync(int minVolume, int maxVolume, CancellationToken cancellationToken)
    {
        return await _context.ContainerTypes
            .AsNoTracking()
            .Where(ct => ct.Volume >= minVolume && ct.Volume <= maxVolume)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductType>> GetCompatibleProductTypesAsync(int containerTypeId, CancellationToken cancellationToken)
    {
        return await _context.ContainerTypeProductTypes
            .AsNoTracking()
            .Where(ctpt => ctpt.ContainerTypeId == containerTypeId && ctpt.ProductType != null)
            .Select(ctpt => ctpt.ProductType!)
            .ToListAsync(cancellationToken);
    }
}