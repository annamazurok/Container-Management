namespace Domain.Entities;

public class Unit : AuditableEntity
{
    public int Id { get; }
    public string Title { get; private set; }
    public UnitType UnitType { get; private set; }

    private Unit(
        int id, string title, UnitType unitType,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Id = id;
        Title = title;
        UnitType = unitType;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static Unit New(int id, string title, UnitType unitType, int createdBy)
        => new Unit(id, title, unitType, DateTime.Now, null, createdBy, null);

    public void UpdateDetails(string title, UnitType unitType, int changedBy)
    {
        Title = title;
        UnitType = unitType;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}