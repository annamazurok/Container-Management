using Domain.Entities;

namespace Api.Dtos;

public record ProductTypeDto(
    int Id,
    string Title)
{
    public static ProductTypeDto FromDomainModel(ProductType model)
        => new(model.Id, model.Title);
}

public record CreateProductTypeDto(
    string Title);