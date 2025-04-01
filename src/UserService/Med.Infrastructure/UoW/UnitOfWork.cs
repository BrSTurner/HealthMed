using Med.Infrastructure.Data;
using Med.SharedKernel.UoW;

namespace Med.Infrastructure.UoW
{
    public class UnitOfWork(UserContext userContext) : IUnitOfWork
    {
        private readonly UserContext _userContext = userContext;

        public async Task<bool> SaveChanges()
        {
            try
            {
                var affectedRows = await _userContext.SaveChangesAsync();
                return affectedRows > 0;
            }
            catch
            {
                return false;   
            }
        }
    }
}
