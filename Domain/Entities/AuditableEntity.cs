using Domain.Interfaces;

namespace Domain.Entities;

public abstract class AuditableEntity : IAuditable
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    public int CreatedBy { get; protected set; }
    public int? ChangedBy { get; protected set; }
}