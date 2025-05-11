using Med.Application.Services;
using Shouldly;

namespace Med.Auth.Tests
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _service = new();

        [Fact(DisplayName = "Non empty Hash Password")]
        [Trait("Password Service", "Hash")]
        public void HashPassword_ShouldReturnNonEmptyHash()
        {
            // Arrange
            var plainPassword = "MySecret123!";

            // Act
            var hash = _service.HashPassword(plainPassword);

            // Assert
            hash.ShouldNotBeNullOrWhiteSpace();
            hash.ShouldNotBe(plainPassword);
        }

        [Fact(DisplayName = "Correct Password")]
        [Trait("Password Service", "Hash")]
        public void IsPasswordValid_ShouldReturnTrue_ForCorrectPassword()
        {
            // Arrange
            var password = "Test@123";
            var hashed = _service.HashPassword(password);

            // Act
            var isValid = _service.IsPasswordValid(password, hashed);

            // Assert
            isValid.ShouldBeTrue();
        }

        [Fact(DisplayName = "Incorrect Password")]
        [Trait("Password Service", "Hash")]
        public void IsPasswordValid_ShouldReturnFalse_ForIncorrectPassword()
        {
            // Arrange
            var correctPassword = "Correct@123";
            var wrongPassword = "Wrong@123";
            var hashed = _service.HashPassword(correctPassword);

            // Act
            var isValid = _service.IsPasswordValid(wrongPassword, hashed);

            // Assert
            isValid.ShouldBeFalse();
        }
    }
}
