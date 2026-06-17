using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace VirtualLED.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken()
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, Environment.GetEnvironmentVariable("AuthorizedUser") ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? string.Empty));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
