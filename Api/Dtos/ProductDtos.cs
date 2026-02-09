using Domain.Entities;

namespace Api.Dtos;

public record ProductDto(
    int Id,
    int TypeId,
    DateTime Produced,
    DateTime? ExpirationDate,
    string? Description,
    DateTime CreatedAt,
    int CreatedBy,
    DateTime? UpdatedAt,
    int? ChangedBy)
{
    public static ProductDto FromDomainModel(Product model)
        => new(
            model.Id,
            model.TypeId,
            model.Produced,
            model.ExpirationDate,
            model.Description,
            model.CreatedAt,
            model.CreatedBy,
            model.UpdatedAt,
            model.ChangedBy);
}

public record CreateProductDto(
    int TypeId,
    DateTime Produced,
    DateTime? ExpirationDate,
    string? Description);

public record UpdateProductDto(
    int Id,
    int TypeId,
    DateTime Produced,
    DateTime? ExpirationDate,
    string? Description);