using Domain;
using Domain.Entities;

namespace Api.Dtos;

public record HistoryDto(
    int Id,
    int ContainerId,
    int? Quantity,
    int? UnitId,
    int? ProductId,
    Status ActionType,
    DateTime FromDate,
    string? Notes,
    int CreatedBy,
    DateTime CreatedAt,
    int? ModifiedBy,
    DateTime? ModifiedAt
)
{
    public static HistoryDto FromDomainModel(History model)
    => new(model.Id,  model.ContainerId, model.Quantity, 
        model.UnitId,  model.ProductId, model.ActionType, 
        model.FromDate, model.Notes, model.CreatedBy, model.CreatedAt,
        model.ChangedBy, model.UpdatedAt);
}