using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Domain;
using LanguageExt;

namespace Api.Services.Implementation;

public class HistoryControllerService(
    IHistoryQueries historyQueries) : IHistoryControllerService
{
    public async Task<IReadOnlyList<HistoryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetAllAsync(cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }

    public async Task<Option<HistoryDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetByIdAsync(id, cancellationToken);

        return result.Match(
            h => HistoryDto.FromDomainModel(h),
            () => Option<HistoryDto>.None);
    }

    public async Task<IReadOnlyList<HistoryDto>> GetByContainerAsync(int containerId, CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetByContainerAsync(containerId, cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<HistoryDto>> GetByProductAsync(int productId, CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetByProductAsync(productId, cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<HistoryDto>> GetByActionTypeAsync(Status actionType, CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetByActionTypeAsync(actionType, cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<HistoryDto>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetByDateRangeAsync(fromDate, toDate, cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<HistoryDto>> GetByUserAsync(int userId, CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetByUserAsync(userId, cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<HistoryDto>> GetRecentHistoryAsync(int count, CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetRecentHistoryAsync(count, cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<HistoryDto>> GetContainerHistoryByDateAsync(int containerId, DateTime fromDate, DateTime toDate,
        CancellationToken cancellationToken)
    {
        var result = await historyQueries.GetContainerHistoryByDateAsync(containerId, fromDate, toDate, cancellationToken);
        
        return result.Select(HistoryDto.FromDomainModel).ToList();
    }
}