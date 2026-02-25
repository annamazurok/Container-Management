using Domain.Interfaces;

namespace Domain.Entities;

public class User : AuditableEntity, IEntity
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string? FathersName { get; private set; }
    public int RoleId { get; private set; }
    public bool Confirmed { get; private set; }
    public string? GoogleId { get; private set; }

    public Role? Role { get; private set; }

    private User(
        string email, string name, string surname,
        string? fathersName, int roleId, bool confirmed,
        string? googleId, 
        DateTime createdAt, DateTime? updatedAt, int createdBy, int? changedBy)
    {
        Email = email;
        Name = name;
        Surname = surname;
        FathersName = fathersName;
        RoleId = roleId;
        Confirmed = confirmed;
        GoogleId = googleId; 
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static User New(
        string email, string name, string surname,
        string? fathersName, int roleId, int createdBy, string? googleId = null)
        => new User(email, name, surname, fathersName, roleId, false,
            googleId, DateTime.Now, null, createdBy, null);

    public void UpdateProfile(string email, string name, string surname, string? fathersName, int changedBy)
    {
        Email = email;
        Name = name;
        Surname = surname;
        FathersName = fathersName;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }

    public void ConfirmEmail(int changedBy)
    {
        Confirmed = true;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }

    public void ChangeRole(int roleId, int changedBy)
    {
        RoleId = roleId;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }

    public void SetGoogleId(string googleId, int changedBy)
    {
        GoogleId = googleId;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
    }
}