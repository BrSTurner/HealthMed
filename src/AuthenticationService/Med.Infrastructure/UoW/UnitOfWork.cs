using Med.Infrastructure.Data;
using Med.SharedKernel.UoW;

namespace Med.Infrastructure.UoW
{
    public class UnitOfWork(AuthContext authContext) : IUnitOfWork
    {
        private readonly AuthContext _authContext = authContext;

        public async Task<bool> SaveChanges()
        {
            try
            {
                var affectedRows = await _authContext.SaveChangesAsync();
                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
