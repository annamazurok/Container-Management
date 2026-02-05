using Domain.Interfaces;

namespace Domain.Entities;

public class Container : AuditableEntity, IEntity
{
    public int Id { get; }
    public string Name { get; private set; }
    public int TypeId { get; private set; }
    public int ProductId { get; private set; }
    public Status Status { get; private set; }
    public DateTime? ChangingDate { get; private set; }
    public int Quantity { get; private set; }
    public int UnitId { get; private set; }
    public string? Notes { get; private set; }

    public ContainerType? Type { get; private set; }
    public Product? Product { get; private set; }
    public Unit? Unit { get; private set; }

    private Container(
        int id, string name, int typeId, int productId,
        Status status, DateTime? changingDate, int quantity,
        int unitId, string? notes,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Id = id;
        Name = name;
        TypeId = typeId;
        ProductId = productId;
        Status = status;
        ChangingDate = changingDate;
        Quantity = quantity;
        UnitId = unitId;
        Notes = notes;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static Container New(
        int id, string name, int typeId, int productId,
        int quantity, int unitId, string? notes, int createdBy)
        => new Container(id, name, typeId, productId, 
            Status.Active, null, quantity, unitId, notes,
            DateTime.Now, null, createdBy, null);

    public void UpdateDetails(
        string name, int typeId, int productId,
        int quantity, int unitId, string? notes, int changedBy)
    {
        Name = name;
        TypeId = typeId;
        ProductId = productId;
        Quantity = quantity;
        UnitId = unitId;
        Notes = notes;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }

    public void ChangeStatus(Status status, int changedBy)
    {
        Status = status;
        ChangingDate = DateTime.Now;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}
