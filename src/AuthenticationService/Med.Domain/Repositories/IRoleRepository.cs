using Med.Domain.Entities;

namespace Med.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesById(params int[] Ids);
    }
}
