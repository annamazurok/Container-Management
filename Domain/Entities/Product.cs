using Domain.Interfaces;

namespace Domain.Entities;

public class Product : AuditableEntity, IEntity
{
    public int Id { get; private set; }
    public int TypeId { get; private set; }
    public DateTime Produced { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public string? Description { get; private set; }

    public ProductType? Type { get; private set; }

    private Product(
        int typeId, DateTime produced,
        DateTime? expirationDate, string? description,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        TypeId = typeId;
        Produced = produced;
        ExpirationDate = expirationDate;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static Product New(
        int typeId, DateTime produced,
        DateTime? expirationDate, string? description, int createdBy)
        => new Product(typeId, produced, expirationDate, description, 
            DateTime.Now, null, createdBy, null);

    public void UpdateDetails(
        int typeId, DateTime produced, DateTime? expirationDate, 
        string? description, int changedBy)
    {
        TypeId = typeId;
        Produced = produced;
        ExpirationDate = expirationDate;
        Description = description;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}