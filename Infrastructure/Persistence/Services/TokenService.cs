using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Persistence.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
            new("userId", user.Id.ToString()),
            new("email", user.Email),
            new("name", user.Name),
            new("surname", user.Surname),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.Role != null)
        {
            claims.Add(new Claim("role", user.Role.Name));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key not configured")));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}