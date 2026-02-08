using Api.Dtos;
using Domain;

namespace Api.Services.Abstract;

public interface IHistoryControllerService : IControllerService<HistoryDto>
{
    Task<IReadOnlyList<HistoryDto>> GetByContainerAsync(int containerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<HistoryDto>> GetByProductAsync(int productId, CancellationToken cancellationToken);
    Task<IReadOnlyList<HistoryDto>> GetByActionTypeAsync(Status actionType, CancellationToken cancellationToken);
    Task<IReadOnlyList<HistoryDto>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
    Task<IReadOnlyList<HistoryDto>> GetByUserAsync(int userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<HistoryDto>> GetRecentHistoryAsync(int count, CancellationToken cancellationToken);
    Task<IReadOnlyList<HistoryDto
    >> GetContainerHistoryByDateAsync(int containerId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
}