using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.SharedKernel.Enumerations;
using Shouldly;

namespace Med.Auth.Tests
{
    public class UserTests
    {
        [Fact(DisplayName = "Non empty Hash Password")]
        [Trait("Password Service", "Hash")]
        public void AddRoles_ShouldAssignDoctorRole_WhenTypeIsDoctor()
        {
            // Arrange
            var user = new User
            {
                Username = "123456/SP",
                PasswordHash = "pass",
                Type = UserType.Doctor
            };

            // Act
            user.AddRoles();

            // Assert
            user.Roles.ShouldNotBeNull();
            user.Roles.Count.ShouldBe(1);
            user.Roles.First().RoleId.ShouldBe((int)RolesEnum.Doctor);
        }

        [Fact]
        public void AddRoles_ShouldAssignPatientRole_WhenTypeIsPatient()
        {
            // Arrange
            var user = new User
            {
                Username = "john",
                PasswordHash = "pass",
                Type = UserType.Patient
            };

            // Act
            user.AddRoles();

            // Assert
            user.Roles.ShouldNotBeNull();
            user.Roles.Count.ShouldBe(1);
            user.Roles.First().RoleId.ShouldBe((int)RolesEnum.Patient);
        }

        [Fact]
        public void AddRoles_ShouldThrow_WhenTypeIsInvalid()
        {
            // Arrange
            var user = new User
            {
                Username = "nobody",
                PasswordHash = "pass",
                Type = (UserType)999 // invalid enum value
            };

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => user.AddRoles());
        }

        [Fact]
        public void Validate_ShouldReturnValidationResult()
        {
            // Arrange
            var user = new User
            {
                Username = "validuser",
                PasswordHash = "validpass",
                Type = UserType.Doctor
            };

            // Act
            var result = user.Validate();

            // Assert
            result.ShouldNotBeNull();
            result.IsValid.ShouldBeTrue(); // Only works if validation passes
        }
    }
}
