namespace Domain.Interfaces;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }

    int CreatedBy { get; }
    int? ChangedBy { get; }
}
