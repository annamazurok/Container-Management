using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Settings;
using Domain.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ProductTypeRepository : BaseRepository<ProductType>, IRepository<ProductType>, IProductTypeQueries
{
    private readonly ApplicationDbContext _context;

    public ProductTypeRepository(ApplicationDbContext context, ApplicationSettings settings) : base(context, settings)
    {
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

    public async Task<Option<ProductType>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypes
            .AsNoTracking()
            .SingleOrDefaultAsync(pt => pt.Title == title, cancellationToken);

        return entity ?? Option<ProductType>.None;
    }
}