using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiBlog.Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiBlog.Infrastructure;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly JwtTokenSettings _authSettings;
        
    public JwtTokenGenerator(IOptions<JwtTokenSettings> authSettings)
    {
        _authSettings = authSettings.Value;
    }
    
    public string GenerateToken(string username)
    {
        var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, username)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = "https://id.com",
            Audience = "https://posts.com",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public record JwtTokenSettings
{
    public string Secret { get; set; }
};