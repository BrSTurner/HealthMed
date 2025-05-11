using Med.Application.Services;
using Med.Domain.Entities;
using Med.SharedKernel.DomainObjects;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Shouldly;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Med.Auth.Tests
{
    public class TokenServiceTests
    {
        private readonly IConfiguration _config;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _config = Substitute.For<IConfiguration>();
            _config["Jwt:Key"].Returns("W3K1x46456htfh4jygk548ukfghjd6567jgfNB0a9iXs=");
            _config["Jwt:Issuer"].Returns("test-issuer");
            _config["Jwt:Audience"].Returns("test-audience");

            _tokenService = new TokenService(_config);
        }

        [Fact(DisplayName = "Generate Token")]
        [Trait("Token Service", "Jwt")]
        public void GenerateToken_ShouldIncludeUserClaimsAndRoles()
        {
            // Arrange
            var user = new User
            {
                Username = "bruno",
                Email = new Email("bruno@example.com"),
                PasswordHash = ""
            };

            var roles = new List<Role>
            {
                new() { Name = "Patient" }
            };

            // Act
            var token = _tokenService.GenerateToken(user, roles);

            // Assert
            token.ShouldNotBeNullOrWhiteSpace();

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            jwt.Issuer.ShouldBe("test-issuer");
            jwt.Audiences.ShouldContain("test-audience");

            var claims = jwt.Claims.ToList();
            claims.ShouldContain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "bruno");
            claims.ShouldContain(c => c.Type == ClaimTypes.Email && c.Value == "bruno@example.com");
            claims.Count(c => c.Type == ClaimTypes.Role).ShouldBe(1);
            claims.ShouldContain(c => c.Type == ClaimTypes.Role && c.Value == "Patient");
        }
    }
}