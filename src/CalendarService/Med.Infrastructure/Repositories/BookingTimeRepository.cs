using Med.Domain.Entities;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class BookingTimeRepository : IBookingTimeRepository
    {
        private readonly CalendarContext _context;
        private readonly DbSet<BookingTime> _entity;
        public BookingTimeRepository(CalendarContext context)
        {
            _context = context;
            _entity = _context.Set<BookingTime>();
        }

        public async Task CreateCalendarBookingTime(List<BookingTime> bookingTimes)
        {
            await _entity.AddRangeAsync(bookingTimes);
        }

        public async Task<BookingTime?> GetBookingTimeById(Guid id)
        {
            return await _entity.FindAsync(id);
        }
    }
}
