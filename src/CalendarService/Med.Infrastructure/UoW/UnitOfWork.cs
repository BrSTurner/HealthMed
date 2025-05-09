using Med.Infrastructure.Data;
using Med.SharedKernel.UoW;

namespace Med.Infrastructure.UoW
{
    public class UnitOfWork(CalendarContext calendarContext) : IUnitOfWork
    {
        private readonly CalendarContext _calendarContext = calendarContext;

        public async Task<bool> SaveChanges()
        {
            try
            {
                var affectedRows = await _calendarContext.SaveChangesAsync();
                return affectedRows > 0;
            }
            catch
            {
                return false;   
            }
        }
    }
}
