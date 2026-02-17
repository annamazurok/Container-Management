using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ContainerRepository : BaseRepository<Container>, IRepository<Container>, IContainerQueries
{
    private readonly ApplicationDbContext _context;


    public ContainerRepository(ApplicationDbContext context, ApplicationSettings settings) : base(context, settings)
    {
        _context = context;
    }
    
    public new async Task<IReadOnlyList<Container>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Containers
            .Include(x => x.Product)
            .Include(x => x.Type)
            .Include(x => x.Unit)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Container> CreateAsync(Container entity, CancellationToken cancellationToken)
    {
        await _context.Containers.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Container> UpdateAsync(Container entity, CancellationToken cancellationToken)
    {
        _context.Containers.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Container> DeleteAsync(Container entity, CancellationToken cancellationToken)
    {
        _context.Containers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Container> DeleteRangeAsync(IEnumerable<Container> entities, CancellationToken cancellationToken)
    {
        _context.Containers.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }

    public async Task<IReadOnlyList<Container>> GetByStatusAsync(Status status, CancellationToken cancellationToken)
    {
        return await _context.Containers
            .AsNoTracking()
            .Where(c => c.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Container>> GetByContainerTypeAsync(int containerTypeId, CancellationToken cancellationToken)
    {
        return await _context.Containers
            .AsNoTracking()
            .Where(c => c.TypeId == containerTypeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Container>> GetByProductAsync(int productId, CancellationToken cancellationToken)
    {
        return await _context.Containers
            .AsNoTracking()
            .Where(c => c.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Container>> GetExpiringContainersAsync(DateTime beforeDate, CancellationToken cancellationToken)
    {
        return await _context.Containers
            .AsNoTracking()
            .Include(c => c.Product)
            .Where(c => c.Product != null && c.Product.ExpirationDate != null && c.Product.ExpirationDate <= beforeDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Container>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.Containers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

        return entity ?? Option<Container>.None;
    }

    public async Task<IReadOnlyList<Container>> GetContainersByProductTypeAsync(int productTypeId, CancellationToken cancellationToken)
    {
        return await _context.Containers
            .AsNoTracking()
            .Include(c => c.Product)
            .Where(c => c.Product != null && c.Product.TypeId == productTypeId)
            .ToListAsync(cancellationToken);
    }
}