
using LanguageExt;

namespace Api.Services.Abstract;

public interface IControllerService<T>
{
    Task<IReadOnlyList<T>> GetAllAsync(
        CancellationToken cancellationToken);
    
    Task<Option<T>> GetByIdAsync( int id,
        CancellationToken cancellationToken);
}