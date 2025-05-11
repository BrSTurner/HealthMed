using Med.Application.Interfaces.Services;
using Med.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Med.Application.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private readonly IConfiguration _config = config;

        public string GenerateToken(User user, List<Role> roles)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Username),
                new(ClaimTypes.Email, user.Email?.Address ?? string.Empty)
            };

            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.Name ?? string.Empty)));           

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
