using Med.Domain.Entities;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface ICalendarRepository : IRepository<Calendar>
    {
        Task<Calendar?> GetCalendarById(Guid id);

        Task<Calendar?> GetCalendarByDoctorId(Guid doctorId);

        Task CreateDoctorCalendar(Calendar calendar);
        Task CreateBookingTime(List<BookingTime> bookingTimes);
    }
}
