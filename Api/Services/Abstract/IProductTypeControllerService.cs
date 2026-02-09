using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract;

public interface IProductTypeControllerService : IControllerService<ProductTypeDto>
{
    Task<Option<ProductTypeDto>> GetByTitleAsync(string title, CancellationToken cancellationToken);
}