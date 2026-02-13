using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ProductRepository : BaseRepository<Product>, IRepository<Product>, IProductQueries
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context, ApplicationSettings settings) :  base(context, settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        
        _context = context;
    }

    public async Task<Product> CreateAsync(Product entity, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Product> UpdateAsync(Product entity, CancellationToken cancellationToken)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Product> DeleteAsync(Product entity, CancellationToken cancellationToken)
    {
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Product> DeleteRangeAsync(IEnumerable<Product> entities, CancellationToken cancellationToken)
    {
        _context.Products.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }

    public async Task<IReadOnlyList<Product>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.TypeId == typeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetExpiredProductsAsync(DateTime currentDate, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.ExpirationDate != null && p.ExpirationDate < currentDate)
            .ToListAsync(cancellationToken);
    }
}