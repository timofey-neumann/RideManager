using System.Text;
using Domain.Interfaces;
using BLL.Configuration;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BLL.Services;

public class AuthService(IAuthRepository repository, IOptions<JwtSettings> jwtSettings) : IAuthService
{
    public async Task<(string token, string role)?> GenerateTokenByEmailAsync(string email)
    {
        var exists = await repository.GetRoleByEmailAsync(email);
        if (exists != null)
        {
            var role = (bool)exists ? "Coordinator" : "Employee";

            var token = GenerateJwtToken(email, role);
            return (token, role);
        }
        else
        {
            return null;
        }
    }

    private string GenerateJwtToken(string email, string role)
    {
        var expirationTime = DateTime.UtcNow.AddDays(1);

        var claims = new[]
        {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "RideManager_AuthService",
            audience: "RideManagerApp",
            claims: claims,
            expires: expirationTime,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
