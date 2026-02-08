using Domain;
using Domain.Entities;
using LanguageExt;

namespace Api.Dtos;

public record ContainerDto(
    int Id,
    string Name,
    string Code,
    int  TypeId,
    int? ProductId,
    Status Status,
    DateTime ChangingDate,
    int? Quantity,
    int? UnitId,
    string? Notes,
    DateTime CreatedAt,
    int CreatedBy, 
    DateTime? UpdatedAt,
    int? ChangedBy)
{
    public static ContainerDto FromDomainModel(Container model)
        => new(
            model.Id, model.Name, model.Code, model.TypeId,
            model.ProductId, model.Status, model.ChangingDate,
            model.Quantity, model.UnitId, model.Notes, model.CreatedAt,
            model.CreatedBy, model.UpdatedAt, model.ChangedBy);
}

public record CreateContainerDto(
    string Name,
    string Code,
    int  TypeId,
    int? ProductId,
    Status Status,
    DateTime ChangingDate,
    int? Quantity,
    int? UnitId,
    string? Notes);
    
public record UpdateContainerDto(
    int Id,
    string Name,
    string Code,
    int  TypeId,
    int? ProductId,
    Status Status,
    DateTime ChangingDate,
    int? Quantity,
    int? UnitId,
    string? Notes);