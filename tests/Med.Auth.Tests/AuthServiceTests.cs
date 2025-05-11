using Med.Application.Interfaces.Services;
using Med.Application.Services;
using Med.Domain.Entities;
using Med.Domain.Exceptions;
using Med.Domain.Repositories;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.UoW;
using NSubstitute;
using Shouldly;

namespace Med.Auth.Tests
{
    public class AuthServiceTests
    {
        private readonly IUserRepository _userRepo = Substitute.For<IUserRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IPasswordService _passwordService = Substitute.For<IPasswordService>();
        private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
        private readonly IRoleRepository _roleRepo = Substitute.For<IRoleRepository>();

        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(_userRepo, _unitOfWork, _passwordService, _tokenService, _roleRepo);
        }

        [Fact(DisplayName = "Create User")]
        [Trait("Auth Service", "User")]
        public async Task CreateUser_ShouldCreate_WhenUserIsValidAndDoesNotExist()
        {
            // Arrange
            var user = TestData.ValidUser();
            _userRepo.GetByEmailOrUsernameAsync(user.Username, user.Email.Address).Returns((User)null);
            _passwordService.HashPassword(user.PasswordHash).Returns("hashed");

            // Act
            var result = await _authService.CreateUser(user);

            // Assert
            result.ShouldBe(user.Id);
            await _userRepo.Received(1).AddAsync(user);
            await _unitOfWork.Received(1).SaveChanges();
            user.PasswordHash.ShouldBe("hashed");
        }

        [Fact(DisplayName = "User Already Exists")]
        [Trait("Auth Service", "User")]
        public async Task CreateUser_ShouldThrow_WhenUserAlreadyExists()
        {
            // Arrange
            var user = TestData.ValidUser();
            _userRepo.GetByEmailOrUsernameAsync(user.Username, user.Email.Address).Returns(user);

            // Act & Assert
            await Should.ThrowAsync<UserAlreadyCreatedException>(() => _authService.CreateUser(user));
        }

        [Fact(DisplayName = "Authenticate User")]
        [Trait("Auth Service", "User")]
        public async Task Authenticate_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = TestData.ValidUser();
            _userRepo.GetByEmailOrUsernameAsync(user.Username, user.Username).Returns(user);
            _passwordService.IsPasswordValid("password", user.PasswordHash).Returns(true);

            var roles = new List<Role> { new() { Name = "Admin" } };
            _roleRepo.GetRolesById(Arg.Any<int[]>()).Returns(roles);
            _tokenService.GenerateToken(user, roles).Returns("token");

            // Act
            var token = await _authService.Authenticate(user.Username, "password");

            // Assert
            token.ShouldBe("token");
        }

        [Fact(DisplayName = "Authenticate User not found")]
        [Trait("Auth Service", "User")]
        public async Task Authenticate_ShouldThrow_WhenUserNotFound()
        {
            // Arrange
            _userRepo.GetByEmailOrUsernameAsync("missing", "missing").Returns((User)null);

            // Act & Assert
            await Should.ThrowAsync<UnauthorizedAccessException>(() => _authService.Authenticate("missing", "password"));
        }

        [Fact(DisplayName = "Authenticate User Password Invalid")]
        [Trait("Auth Service", "User")]
        public async Task Authenticate_ShouldThrow_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = TestData.ValidUser();
            _userRepo.GetByEmailOrUsernameAsync(user.Username, user.Username).Returns(user);
            _passwordService.IsPasswordValid("wrong", user.PasswordHash).Returns(false);

            // Act & Assert
            await Should.ThrowAsync<UnauthorizedAccessException>(() => _authService.Authenticate(user.Username, "wrong"));
        }
    }

    // Support test data
    public static class TestData
    {
        public static User ValidUser()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Type = SharedKernel.Enumerations.UserType.Patient,
                Username = "111.444.777-35",
                Email = new Email("test@example.com"),
                PasswordHash = "plainpass",
                Roles =
                [
                    new() { RoleId = 1 }
                ]
            };

            user.Validate().IsValid.ShouldBeTrue(); // sanity check

            return user;
        }
    }
}
