using Domain.Interfaces;

namespace Domain.Entities;

public class Unit : AuditableEntity, IEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public UnitType UnitType { get; private set; }

    private Unit(
        string title, UnitType unitType,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Title = title;
        UnitType = unitType;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static Unit New(string title, UnitType unitType, int createdBy)
        => new Unit(title, unitType, DateTime.Now, null, createdBy, null);

    public void UpdateDetails(string title, UnitType unitType, int changedBy)
    {
        Title = title;
        UnitType = unitType;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}