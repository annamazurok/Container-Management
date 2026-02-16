using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ContainerTypeRepository : BaseRepository<ContainerType>, IRepository<ContainerType>, IContainerTypeQueries
{
    private readonly ApplicationDbContext _context;

    public ContainerTypeRepository(ApplicationDbContext context, 
        ApplicationSettings settings) : base(context, settings)
    {
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
    
    public async Task<Option<ContainerType>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.ContainerTypes
            .AsNoTracking()
            .SingleOrDefaultAsync(ct => ct.Name == name, cancellationToken);

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