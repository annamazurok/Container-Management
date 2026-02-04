using System.ComponentModel;

namespace Application.Common.Interfaces.Repositories;

public interface IContainerRepository
{
    public interface IContainerRepository
    {
        Task<Container> AddAsync(Container entity, CancellationToken cancellationToken);
        Task<Container> UpdateAsync(Container entity, CancellationToken cancellationToken);
        Task<Container> DeleteAsync(Container entity, CancellationToken cancellationToken);
    }
}