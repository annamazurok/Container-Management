using Domain.Interfaces;

namespace Domain.Entities;

public class ContainerType : AuditableEntity, IEntity
{
    public int Id { get; }
    public string Name { get; private set; }
    public int Volume { get; private set; }
    public int UnitId { get; private set; }

    public Unit? Unit { get; private set; }

    private ContainerType(
        int id, string name, int volume, int unitId,
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Id = id;
        Name = name;
        Volume = volume;
        UnitId = unitId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static ContainerType New(
        int id, string name, int volume, int unitId)
        => new ContainerType(id, name, volume, unitId, DateTime.Now, null, 1 ,null);

    public void UpdateDetails(string name, int volume, int unitId)
    {
        Name = name;
        Volume = volume;
        UnitId = unitId;
        UpdatedAt = DateTime.Now;
        ChangedBy = 1;
    }
}