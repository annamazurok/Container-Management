namespace Domain.Entities;

public class Product :  AuditableEntity
{
    public int Id { get; }
    public int TypeId { get; private set; }
    public DateTime Produced { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public string? Description { get; private set; }

    public ProductType? Type { get; private set; }

    private Product(
        int id, int typeId, DateTime produced,
        DateTime? expirationDate, string? description,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Id = id;
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
        int id, int typeId, DateTime produced,
        DateTime? expirationDate, string? description)
        => new Product(id, typeId, produced, expirationDate, description, DateTime.Now, null, 1, null);

    public void UpdateDetails(
        DateTime produced, DateTime? expirationDate, string? description)
    {
        Produced = produced;
        ExpirationDate = expirationDate;
        Description = description;
        UpdatedAt = DateTime.Now;
        ChangedBy = 1;
    }

    public void ChangeType(int typeId)
    {
        TypeId = typeId;
        UpdatedAt = DateTime.Now;
        ChangedBy = 1;
    }
}