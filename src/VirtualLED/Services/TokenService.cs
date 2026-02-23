using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VirtualLED.Models;

namespace VirtualLED.Services;

public class TokenService : ITokenService
{
    private readonly string _key;
    public TokenService(IOptions<AppSettings> options)
    {
        _key = options.Value.EncryptionKey;
    }

    public string GenerateToken()
    {
        var keyBytes = Encoding.UTF8.GetBytes(_key);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
