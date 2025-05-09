using Med.Infrastructure.Data;
using Med.SharedKernel.UoW;

namespace Med.Infrastructure.UoW
{
    public class UnitOfWork(AppointmentContext appointmentContext) : IUnitOfWork
    {
        private readonly AppointmentContext _appointmentContext = appointmentContext;

        public async Task<bool> SaveChanges()
        {
            try
            {
                var affectedRows = await _appointmentContext.SaveChangesAsync();
                return affectedRows > 0;
            }
            catch
            {
                return false;   
            }
        }
    }
}
