using Domain.Entities;

namespace Api.Dtos;

public record ContainerTypeDto(
    int Id,
    string Name,
    int Volume,
    int UnitId,
    IReadOnlyList<ContainerTypeProductTypeDto>? ProductTypes)
{
    public static ContainerTypeDto FromDomainModel(ContainerType model)
        => new(
            model.Id,
            model.Name,
            model.Volume,
            model.UnitId,
            model.ProductTypes != null
                ? model.ProductTypes
                    .Select(ContainerTypeProductTypeDto.FromDomainModel)
                    .ToList()
                : []);
}

public record CreateContainerTypeDto(
    string Name,
    int Volume,
    int UnitId,
    IReadOnlyList<int> ProductTypeIds);

public record UpdateContainerTypeDto(
    int Id,
    string Name,
    int Volume,
    int UnitId,
    IReadOnlyList<int> ProductTypeIds);
