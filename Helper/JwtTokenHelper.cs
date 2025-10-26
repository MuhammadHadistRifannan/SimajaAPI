using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimajaAPI;

public class JwtTokenHelper
{
    private readonly IConfiguration _configuration;

    public JwtTokenHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string id , string username , string role)
    {
        var secretKey = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier , id) ,
                new Claim(ClaimTypes.Name, username) ,
                new Claim(ClaimTypes.Role , role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryIn"]!)),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public dynamic DecodeToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        if (jwtHandler.CanReadToken(token))
        {
            var result = jwtHandler.ReadJwtToken(token);
            var expUnix = Convert.ToInt64(result.Payload["exp"]);

            // Convert Unix timestamp to DateTime
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).ToLocalTime();
            if (DateTime.Now > expirationDate)
            {
                throw new SecurityTokenExpiredException("Token Expired !!");
            }

            return new
            {
                Id = result.Payload.Where(e => e.Key == "nameid").FirstOrDefault().Value,
                Username = result.Payload.Where(e => e.Key == "unique_name").FirstOrDefault().Value,
                Issuer = result.Payload.Where(e => e.Key == "iss").FirstOrDefault().Value,
                Audience = result.Payload.Where(e => e.Key == "aud").FirstOrDefault().Value,
                Expiration = result.Payload.Where(e => e.Key == "exp").FirstOrDefault().Value
            };
        }

        return new { };
    }
}
