namespace Domain.Entities;

public class Role
{
    public int Id { get; }
    public string Name { get; private set; }

    private Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Role New(int id, string name)
        => new Role(id, name);

    public void UpdateName(string name)
    {
        Name = name;
    }
}