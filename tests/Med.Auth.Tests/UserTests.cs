using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.SharedKernel.Enumerations;
using Shouldly;

namespace Med.Auth.Tests
{
    public class UserTests
    {
        [Fact(DisplayName = "Assign Doctor role")]
        [Trait("User", "Roles")]
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

        [Fact(DisplayName = "Assign Patient role")]
        [Trait("User", "Roles")]
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

        [Fact(DisplayName = "Invalid type")]
        [Trait("User", "Roles")]
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

        [Fact(DisplayName = "Validate Doctor")]
        [Trait("User", "Roles")]
        public void Validate_ShouldReturnValidationResult()
        {
            // Arrange
            var user = new User
            {
                Username = "1234/SP",
                PasswordHash = "validpass",
                Type = UserType.Doctor,                
            };

            // Act
            var result = user.Validate();

            // Assert
            result.ShouldNotBeNull();
            result.IsValid.ShouldBeTrue(); 
        }
    }
}
