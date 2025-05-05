using MassTransit;
using Med.Application.Interfaces.Services;
using Med.Domain.Entities;
using Med.Domain.Exceptions;
using Med.MessageBus.Integration.Requests.Users;
using Med.MessageBus.Integration.Responses.Users;
using Med.SharedKernel.DomainObjects;

namespace Med.Application.Consumers
{
    public class AuthConsumer(IAuthService authService) : IConsumer<CreateUserRequest>
    {
        private readonly IAuthService _authService = authService;

        public async Task Consume(ConsumeContext<CreateUserRequest> context)
        {
            var response = new CreateUserResponse();

            try
            {
                var user = new User
                {
                    Username = context.Message.Username,
                    PasswordHash = context.Message.Password,
                    Type = context.Message.Type,
                    Email = string.IsNullOrEmpty(context.Message.Email) ? null : Email.Create(context.Message.Email)
                };

                var userId = await _authService.CreateUser(user);
                var token = await _authService.Authenticate(context.Message.Username, context.Message.Password);

                response = new () 
                {
                    UserId = userId,
                    Success = true,
                    Token = token
                };                
            }
            catch (UserAlreadyCreatedException e)
            {
                response = new () { ErrorMessage = e.Message };
            }
            catch (UserInvalidDataException e)
            {
                response = new() { ErrorMessage = e.Message };
            }
            catch (Exception e)
            {
                response = new () { ErrorMessage = $"Algo deu errado ao criar o usuário, {e.Message}" };
            }
            finally
            {
                await context.RespondAsync(response);
            }
        }
    }
}
