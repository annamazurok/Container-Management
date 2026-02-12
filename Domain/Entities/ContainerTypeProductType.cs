using Domain.Interfaces;

namespace Domain.Entities;

public class ContainerTypeProductType 
{
    public int ContainerTypeId { get; private set; }
    public int ProductTypeId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int CreatedBy { get; private set; }

    public ContainerType? ContainerType { get; private set; }
    public ProductType? ProductType { get; private set; }

    private ContainerTypeProductType(int containerTypeId, int productTypeId, int createdBy)
    {
        ContainerTypeId = containerTypeId;
        ProductTypeId = productTypeId;
        CreatedAt = DateTime.Now;
        CreatedBy = createdBy;
    }

    public static ContainerTypeProductType New(int containerTypeId, int productTypeId, int createdBy)
        => new ContainerTypeProductType(containerTypeId, productTypeId, createdBy);
}