using MassTransit;
using Med.Application.Consumers;
using Med.Application.Interfaces.Services;
using Med.Domain.Entities;
using Med.Domain.Exceptions;
using Med.MessageBus.Integration.Requests.Users;
using Med.MessageBus.Integration.Responses.Users;
using Med.SharedKernel.Enumerations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Med.Auth.Tests
{
    public class AuthConsumerTests
    {
        private readonly IAuthService _authService = Substitute.For<IAuthService>();
        private readonly AuthConsumer _consumer;

        public AuthConsumerTests()
        {
            _consumer = new AuthConsumer(_authService);
        }

        [Fact(DisplayName = "Should create user")]
        [Trait("Auth Consumer", "User")]
        public async Task Consume_ShouldRespondWithSuccess_WhenUserIsCreated()
        {
            // Arrange
            var message = new CreateUserRequest
            {
                Username = "111.444.777-35",
                Password = "123",
                Type = UserType.Patient,
                Email = "bruno@email.com"
            };

            var context = Substitute.For<ConsumeContext<CreateUserRequest>>();
            context.Message.Returns(message);

            var expectedUserId = Guid.NewGuid();
            var expectedToken = "jwt-token";

            _authService.CreateUser(Arg.Any<User>()).Returns(expectedUserId);
            _authService.Authenticate(message.Username, message.Password).Returns(expectedToken);

            CreateUserResponse actualResponse = null!;
            await context.RespondAsync(Arg.Do<CreateUserResponse>(r => actualResponse = r));

            // Act
            await _consumer.Consume(context);

            // Assert
            actualResponse.ShouldNotBeNull();
            actualResponse.Success.ShouldBeTrue();
            actualResponse.UserId.ShouldBe(expectedUserId);
            actualResponse.Token.ShouldBe(expectedToken);
            actualResponse.ErrorMessage.ShouldBeNull();
        }

        [Fact(DisplayName = "User already exists")]
        [Trait("Auth Consumer", "User")]
        public async Task Consume_ShouldRespondWithError_WhenUserAlreadyExists()
        {
            // Arrange
            var message = new CreateUserRequest
            {
                Username = "111.444.777-35",
                Password = "123",
                Type = UserType.Patient,
                Email = "bruno@email.com"
            };

            var context = Substitute.For<ConsumeContext<CreateUserRequest>>();
            context.Message.Returns(message);

            _authService.CreateUser(Arg.Any<User>())
                .Throws(new UserAlreadyCreatedException("Usuário já existe"));

            CreateUserResponse actualResponse = null!;
            await context.RespondAsync(Arg.Do<CreateUserResponse>(r => actualResponse = r));

            // Act
            await _consumer.Consume(context);

            // Assert
            actualResponse.Success.ShouldBeFalse();
            actualResponse.ErrorMessage.ShouldBe("Usuário já existe");
            actualResponse.Token.ShouldBeNull();
        }

        [Fact(DisplayName = "User data is invalid")]
        [Trait("Auth Consumer", "User")]
        public async Task Consume_ShouldRespondWithError_WhenUserDataIsInvalid()
        {
            // Arrange
            var context = Substitute.For<ConsumeContext<CreateUserRequest>>();
            context.Message.Returns(new CreateUserRequest { Username = "bad", Password = "x" });

            _authService.CreateUser(Arg.Any<User>())
                .Throws(new UserInvalidDataException("Dados inválidos"));

            CreateUserResponse actualResponse = null!;
            await context.RespondAsync(Arg.Do<CreateUserResponse>(r => actualResponse = r));

            // Act
            await _consumer.Consume(context);

            // Assert
            actualResponse.Success.ShouldBeFalse();
            actualResponse.ErrorMessage.ShouldBe("Dados inválidos");
        }

        [Fact(DisplayName = "Should throw exception")]
        [Trait("Auth Consumer", "User")]
        public async Task Consume_ShouldRespondWithGenericError_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var context = Substitute.For<ConsumeContext<CreateUserRequest>>();
            context.Message.Returns(new CreateUserRequest { Username = "oops", Password = "fail" });

            _authService.CreateUser(Arg.Any<User>())
                .Throws(new Exception("Connection timeout"));

            CreateUserResponse actualResponse = null!;
            await context.RespondAsync(Arg.Do<CreateUserResponse>(r => actualResponse = r));

            // Act
            await _consumer.Consume(context);

            // Assert
            actualResponse.Success.ShouldBeFalse();
            actualResponse.ErrorMessage.ShouldContain("Algo deu errado ao criar o usuário");
            actualResponse.ErrorMessage.ShouldContain("Connection timeout");
        }
    }
}
