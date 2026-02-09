using Domain.Entities;

namespace Api.Dtos;

public record UserDto(
    int Id,
    string Email,
    string Name,
    string Surname,
    string? FathersName,
    int RoleId,
    bool Confirmed,
    DateTime CreatedAt,
    int CreatedBy,
    DateTime? UpdatedAt,
    int? ChangedBy)
{
    public static UserDto FromDomainModel(User model)
        => new(
            model.Id, model.Email, model.Name, model.Surname,
            model.FathersName, model.RoleId, model.Confirmed,
            model.CreatedAt, model.CreatedBy, model.UpdatedAt, model.ChangedBy);
}

public record CreateUserDto(
    string Email,
    string Name,
    string Surname,
    string? FathersName,
    int RoleId);

public record UpdateUserProfileDto(
    int Id,
    string Name,
    string Surname,
    string? FathersName);

public record ChangeUserRoleDto(
    int Id,
    int RoleId);