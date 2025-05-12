using BCrypt.Net;
using Med.Authentication.WebAPI.Inputs;
using Med.Domain.Entities;
using Med.Integration.Tests.Base;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Enumerations;
using System.Net;
using System.Net.Http.Json;

namespace Med.Integration.Tests
{
    public class AuthEndpointTests(WebClientFixture fixture) : IClassFixture<WebClientFixture>
    {
        private readonly WebClientFixture _fixture = fixture;

        [Fact(DisplayName = "Login using CPF")]
        [Trait("Auth Integration", "Login")]
        public async Task Login_CPF_Returns_Token()
        {
            //Arrange
            var user = CreatePatient();
            await _fixture.InsertUser(user);

            var client = _fixture.Client;

            //Act
            var response = await client.PostAsJsonAsync("/api/auth/login", new AuthenticateInput { UsernameOrEmail = user.Username, Password = "Password"});

            //Assert
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<string>();

            Assert.NotNull(token);
        }


        [Fact(DisplayName = "Login using Email")]
        [Trait("Auth Integration", "Login")]
        public async Task Login_Email_Returns_Token()
        {
            //Arrange
            var user = CreatePatient();
            await _fixture.InsertUser(user);

            var client = _fixture.Client;

            //Act
            var response = await client.PostAsJsonAsync("/api/auth/login", new AuthenticateInput { UsernameOrEmail = user.Email?.Address ?? string.Empty, Password = "Password" });

            //Assert
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<string>();

            Assert.NotNull(token);
        }

        [Fact(DisplayName = "Login using CRM")]
        [Trait("Auth Integration", "Login")]
        public async Task Login_CRM_Returns_Token()
        {
            //Arrange
            var user = CreateDoctor();
            await _fixture.InsertUser(user);

            var client = _fixture.Client;

            //Act
            var response = await client.PostAsJsonAsync("/api/auth/login", new AuthenticateInput { UsernameOrEmail = user.Username, Password = "Password" });

            //Assert
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<string>();

            Assert.NotNull(token);
        }

        [Fact(DisplayName = "Login using invalid user")]
        [Trait("Auth Integration", "Login")]
        public async Task Login_Invalid_Returns_Unauthorized()
        {
            //Arrange
            var client = _fixture.Client;

            //Act
            var response = await client.PostAsJsonAsync("/api/auth/login", new AuthenticateInput { UsernameOrEmail = "16843/SP", Password = "Password" });

            //Assert
            var message = await response.Content.ReadFromJsonAsync<string>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(message);
        }


        [Fact(DisplayName = "Login invalid password")]
        [Trait("Auth Integration", "Login")]
        public async Task Login_Invalid_Password_Returns_Unauthorized()
        {
            //Arrange
            var user = CreateDoctor();
            await _fixture.InsertUser(user);
            var client = _fixture.Client;

            //Act
            var response = await client.PostAsJsonAsync("/api/auth/login", new AuthenticateInput { UsernameOrEmail = "12345/SP", Password = "P4ssw0rd$" });

            //Assert
            var message = await response.Content.ReadFromJsonAsync<string>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(message);
        }

        private static User CreatePatient() => new()
        {
            Email = Email.Create("test@test.com"),
            Username = "123.456.789-09",
            Type = UserType.Patient,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password")
        };

        private static User CreateDoctor() => new()
        {            
            Username = "12345/SP",
            Type = UserType.Doctor,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password")
        };
    }
}