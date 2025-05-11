using Med.Domain.Entities;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AuthContext _context;
        private readonly DbSet<Role> _roles;
        
        public RoleRepository(AuthContext context)
        {
            _context = context;
            _roles = _context.Set<Role>();
        }

        public async Task<List<Role>> GetRolesById(params int[] Ids)
            => await _roles.Where(x => Ids.Contains(x.Id)).ToListAsync();
    }
}
