namespace Domain.Entities;

public class ProductType : AuditableEntity
{
    public int Id { get; }
    public string Title { get; private set; }

    private ProductType(
        int id, string title,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Id = id;
        Title = title;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static ProductType New(int id, string title, int createdBy)
        => new ProductType(id, title, DateTime.Now, null, createdBy, null);

    public void UpdateTitle(string title, int changedBy)
    {
        Title = title;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}