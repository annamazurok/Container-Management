using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ContainerTypeProductTypeRepository(ApplicationDbContext context) 
: IContainerTypeProductTypeRepository, IContainerTypeProductTypeQuery
{
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

    public async Task CreateRangeAsync(IReadOnlyList<ContainerTypeProductType> entities, CancellationToken ct)
    {
        await context.ContainerTypeProductTypes.AddRangeAsync(entities, ct);
        await context.SaveChangesAsync(ct);
    }
}