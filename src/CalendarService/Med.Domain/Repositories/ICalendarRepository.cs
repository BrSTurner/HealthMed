using Med.Domain.Entities;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface ICalendarRepository : IRepository<Calendar>
    {
        Task<Calendar?> GetCalendarByDoctor(Guid doctorId);

        void CreateDoctorCalendar(Calendar calendar);
    }
}
