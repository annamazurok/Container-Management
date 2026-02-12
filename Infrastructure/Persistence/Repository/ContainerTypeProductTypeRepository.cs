using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ContainerTypeProductTypeRepository(ApplicationDbContext context)
{
    public async Task<ContainerTypeProductType> CreateAsync(
        ContainerTypeProductType entity,
        CancellationToken cancellationToken)
    {
        await context.ContainerTypeProductTypes.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ContainerTypeProductType> DeleteAsync(
        ContainerTypeProductType entity,
        CancellationToken cancellationToken)
    {
        context.ContainerTypeProductTypes.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ContainerTypeProductType> DeleteRangeAsync(
        IEnumerable<ContainerTypeProductType> entities,
        CancellationToken cancellationToken)
    {
        context.ContainerTypeProductTypes.RemoveRange(entities);
        await context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }

    public async Task<bool> ExistsAsync(
        int containerTypeId,
        int productTypeId,
        CancellationToken cancellationToken)
    {
        return await context.ContainerTypeProductTypes
            .AsNoTracking()
            .AnyAsync(ctpt => ctpt.ContainerTypeId == containerTypeId 
                           && ctpt.ProductTypeId == productTypeId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ContainerTypeProductType>> GetByContainerTypeAsync(
        int containerTypeId,
        CancellationToken cancellationToken)
    {
        return await context.ContainerTypeProductTypes
            .AsNoTracking()
            .Where(ctpt => ctpt.ContainerTypeId == containerTypeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ContainerTypeProductType>> GetByProductTypeAsync(
        int productTypeId,
        CancellationToken cancellationToken)
    {
        return await context.ContainerTypeProductTypes
            .AsNoTracking()
            .Where(ctpt => ctpt.ProductTypeId == productTypeId)
            .ToListAsync(cancellationToken);
    }
}