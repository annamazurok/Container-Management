using Domain.Entities;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IProductTypeQueries : IBaseQuery<ProductType>
{
    Task<Option<ProductType>> GetByTitleAsync(string title, CancellationToken cancellationToken);
}