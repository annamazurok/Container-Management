using Domain.Interfaces;

namespace Domain.Entities;

public class History : AuditableEntity, IEntity
{
    public int Id { get; private set; }
    public int ContainerId { get; private set; }
    public int? Quantity { get; private set; }
    public int? UnitId { get; private set; }
    public int? ProductId { get; private set; }
    public Status ActionType { get; private set; }
    public DateTime FromDate { get; private set; }
    public string? Notes { get; private set; }

    public Container? Container { get; private set; }
    public Unit? Unit { get; private set; }
    public Product? Product { get; private set; }

    private History(
        int containerId, int? quantity, int? unitId,
        int? productId, Status actionType, DateTime fromDate, string? notes,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        ContainerId = containerId;
        Quantity = quantity;
        UnitId = unitId;
        ProductId = productId;
        ActionType = actionType;
        FromDate = fromDate;
        Notes = notes;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static History New(
        int containerId, int? quantity, int? unitId,
        int? productId, Status actionType, DateTime fromDate, string? notes, int createdBy)
        => new History(containerId, quantity, unitId, productId, actionType, fromDate, notes,
            DateTime.Now, null, createdBy, null);
}
