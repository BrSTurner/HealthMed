using Med.Domain.Entities;

namespace Med.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);

        Task<User?> GetByEmailOrUsernameAsync(string username, string email);
    }
}
