using Domain.Entities;

namespace Api.Dtos;

public record ContainerTypeProductTypeDto(ProductTypeDto? ProductType)
{
    public static ContainerTypeProductTypeDto FromDomainModel(ContainerTypeProductType model)
        => new(model.ProductType == null
            ? null
            : ProductTypeDto.FromDomainModel(model.ProductType));
}