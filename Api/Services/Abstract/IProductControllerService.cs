using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract;

public interface IProductControllerService : IControllerService<ProductDto>
{
    Task<IReadOnlyList<ProductDto>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductDto>> GetExpiredProductsAsync(DateTime currentDate, CancellationToken cancellationToken);
}