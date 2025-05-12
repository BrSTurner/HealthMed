using Med.Domain.Entities;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface IBookingTimeRepository : IRepository<BookingTime>
    {
        Task CreateCalendarBookingTime(List<BookingTime> bookingTimes);

        void UpdateCalendarBookingTime(List<BookingTime> bookingTimes);

        Task<BookingTime?> GetBookingTimeById(Guid id);
    }
}
