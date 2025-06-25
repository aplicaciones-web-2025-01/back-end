using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using learning_center_back.Security.Domai_.Entities;
using learning_center_back.Shared.Application.Commands;
using Microsoft.IdentityModel.Tokens;

namespace learning_center_back.Security.Application;

public class JwtEncryptService : IJwtEncryptService
{

    private IConfiguration _configuration;
    private SymmetricSecurityKey _key;

    public JwtEncryptService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:key"]!));
    }

    public string Encrypt(User user)
    {
        var Claims = new[]
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    public User Decrypt(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var handler = new JwtSecurityTokenHandler();


        var principal = handler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        if (validatedToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        var id = principal.FindFirst(ClaimTypes.Sid)?.Value;
        var username = principal.FindFirst(ClaimTypes.Name)?.Value;
        var role = principal.FindFirst(ClaimTypes.Role)?.Value;

        if (int.TryParse(id, out var userId) && username != null && role != null)
        {
            return new User
            {
                Id = userId,
                Username = username,
                Role = role
            };
        }

        return null;
    }
}