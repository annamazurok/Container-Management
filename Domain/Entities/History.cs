using Domain.Interfaces;

namespace Domain.Entities;

public class History : AuditableEntity, IEntity
{
    public int Id { get; }
    public int ContainerId { get; private set; }
    public int Quantity { get; private set; }
    public int UnitId { get; private set; }
    public int ProductId { get; private set; }
    public Status ActionType { get; private set; }
    public DateTime FromDate { get; private set; }

    public Container? Container { get; private set; }
    public Unit? Unit { get; private set; }
    public Product? Product { get; private set; }

    private History(
        int id, int containerId, int quantity, int unitId,
        int productId, Status actionType, DateTime fromDate,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Id = id;
        ContainerId = containerId;
        Quantity = quantity;
        UnitId = unitId;
        ProductId = productId;
        ActionType = actionType;
        FromDate = fromDate;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static History New(
        int id, int containerId, int quantity, int unitId,
        int productId, Status actionType, DateTime fromDate, int createdBy)
        => new History(id, containerId, quantity, unitId, productId, actionType, fromDate,
            DateTime.Now, null, createdBy, null);

    public void UpdateDetails(
        int quantity, DateTime fromDate, int changedBy)
    {
        Quantity = quantity;
        FromDate = fromDate;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }

    public void ChangeActionType(Status actionType, int changedBy)
    {
        ActionType = actionType;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}
