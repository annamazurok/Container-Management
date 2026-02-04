namespace Domain.Entities;

public class Unit : AuditableEntity
{
    public int Id { get; }
    public string Title { get; private set; }
    public UnitType UnitType { get; private set; }

    private Unit(
        int id, string title, UnitType unitType,
        DateTime createdAt, DateTime? updatedAt, int createdBy,  int? updatedBy)
    {
        Id = id;
        Title = title;
        UnitType = unitType;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Unit New(int id, string title, UnitType unitType)
        => new Unit(id, title, unitType, DateTime.Now, null, 1, null);

    public void UpdateDetails(string title, UnitType unitType)
    {
        Title = title;
        UnitType = unitType;
        UpdatedAt = DateTime.Now;
        ChangedBy = 1;
    }
}