namespace Domain.Entities;

public class User : AuditableEntity
{
    public int Id { get; }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string? FathersName { get; private set; }
    public int RoleId { get; private set; }
    public bool Confirmed { get; private set; }

    public Role? Role { get; private set; }
    
    private User(
        int id, string email, string name, string surname, 
        string? fathersName, int roleId, bool confirmed,
        DateTime createdAt, DateTime? updatedAt,
        int createdBy, int changedBy)
    {
        Id = id;
        Email = email;
        Name = name;
        Surname = surname;
        FathersName = fathersName;
        RoleId = roleId;
        Confirmed = confirmed;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CreatedBy = createdBy;
        ChangedBy = changedBy;
    }

    public static User New(
        int id, string email, string name, string surname,
        string? fathersName, int roleId)
        => new User(id, email, name, surname, fathersName, roleId, false, DateTime.Now, null, 1, 1);

    public void UpdateProfile(string name, string surname, string? fathersName)
    {
        Name = name;
        Surname = surname;
        FathersName = fathersName;
        UpdatedAt = DateTime.Now;
        ChangedBy = 1;
    }

    public void ConfirmEmail()
    {
        Confirmed = true;
        UpdatedAt = DateTime.Now;
        ChangedBy = 1;
    }

    public void ChangeRole(int roleId)
    {
        RoleId = roleId;
        UpdatedAt = DateTime.Now;
        ChangedBy = 1;
    }
}