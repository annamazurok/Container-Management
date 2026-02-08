using Domain.Entities;

namespace Api.Dtos;

public record ContainerTypeDto(
    int Id,
    string Name,
    int Volume,
    int UnitId)
{
    public static ContainerTypeDto FromDomainModel(ContainerType model)
    => new(model.Id, model.Name, model.Volume, model.UnitId);
}

public record CreateContainerTypeDto(
    string Name,
    int Volume,
    int UnitId);
