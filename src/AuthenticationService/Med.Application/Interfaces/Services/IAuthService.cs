using Med.Domain.Entities;

namespace Med.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Guid> CreateUser(User user);
        Task<string> Authenticate(string usernameOrEmail, string password);
    }
}
