namespace Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
    int? UserId { get; }

    string? Email { get; }

    bool IsAuthenticated { get; }

    bool IsAdmin { get; }
}