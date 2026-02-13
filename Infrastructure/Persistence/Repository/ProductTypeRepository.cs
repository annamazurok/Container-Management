using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ProductTypeRepository : IRepository<ProductType>, IProductTypeQueries
{
    private readonly ApplicationDbContext _context;

    public ProductTypeRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        
        _context = context;
    }

    public async Task<ProductType> CreateAsync(ProductType entity, CancellationToken cancellationToken)
    {
        await _context.ProductTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ProductType> UpdateAsync(ProductType entity, CancellationToken cancellationToken)
    {
        _context.ProductTypes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ProductType> DeleteAsync(ProductType entity, CancellationToken cancellationToken)
    {
        _context.ProductTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<ProductType> DeleteRangeAsync(IEnumerable<ProductType> entities, CancellationToken cancellationToken)
    {
        _context.ProductTypes.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return entities.FirstOrDefault();
    }

    public async Task<IReadOnlyList<ProductType>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.ProductTypes.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Option<ProductType>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity ?? Option<ProductType>.None;
    }

    public async Task<Option<ProductType>> GetByIdAsync(int? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
            return Option<ProductType>.None;

        return await GetByIdAsync(id.Value, cancellationToken);
    }

    public async Task<Option<ProductType>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Title == title, cancellationToken);

        return entity ?? Option<ProductType>.None;
    }
}