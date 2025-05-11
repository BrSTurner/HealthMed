using Med.Domain.Entities;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface IBookingTimeRepository : IRepository<BookingTime>
    {
        void CreateCalendarBookingTime(List<BookingTime> bookingTimes);

        Task<BookingTime?> GetBookingTimeById(Guid id);
    }
}
