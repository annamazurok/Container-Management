using System.Security.Claims;
using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public int? UserId
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User
                                  .FindFirst("userId")?.Value  
                              ?? httpContextAccessor.HttpContext?.User
                                  .FindFirst(ClaimTypes.NameIdentifier)?.Value; 

            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public string? Email => httpContextAccessor.HttpContext?.User
        .FindFirst(ClaimTypes.Email)?.Value;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsAdmin
    {
        get
        {
            var roles = httpContextAccessor.HttpContext?.User
                .FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return roles?.Contains("Admin") ?? false;
        }
    }
}