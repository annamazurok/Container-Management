using Domain.Interfaces;

namespace Domain.Entities;

public class ProductType : AuditableEntity, IEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }

    private ProductType(
    string title,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Title = title;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static ProductType New(string title, int createdBy)
        => new ProductType(title, DateTime.Now, null, createdBy, null);

    public void UpdateDetails(string title, int changedBy)
    {
        Title = title;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}