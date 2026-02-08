using Domain;
using Domain.Entities;

namespace Api.Dtos;

public record UnitDto(
    int Id,
    string Title,
    UnitType UnitType)
{
    public static UnitDto FromDomainModel(Unit model)
    => new(model.Id, model.Title, model.UnitType);
}

public record CreateUnitDto(
    string Title,
    UnitType UnitType);