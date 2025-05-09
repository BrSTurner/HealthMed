using Med.Domain.Entities;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class UserRepository(AuthContext context) : IUserRepository
    {
        private readonly AuthContext _context = context;
        private readonly DbSet<User> _users = context.Set<User>();
        public async Task<User> AddAsync(User user)
        {
            var entitySet = await _users.AddAsync(user);
            return entitySet.Entity;
        }

        public async Task<User?> GetByEmailOrUsernameAsync(string username, string email)
        {
            return await _users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => 
                    u.Username.ToLower() ==  username.ToLower() || (!string.IsNullOrEmpty(email) && 
                    u.Email.Address.ToLower() == email.ToLower()));
        }
    }
}
