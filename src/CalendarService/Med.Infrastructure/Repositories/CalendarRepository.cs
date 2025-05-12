using Med.Domain.Entities;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly CalendarContext _context;
        private readonly DbSet<Calendar> _entity;
        public CalendarRepository(CalendarContext context)
        {
            _context = context;
            _entity = _context.Set<Calendar>();
        }

        public async Task<Calendar?> GetCalendarById(Guid id)
        {
            return await _entity
                .Include(x => x.Bookings)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateDoctorCalendar(Calendar calendar)
        {
            await _entity.AddAsync(calendar);
        }

        public async Task CreateBookingTime(List<BookingTime> bookingTimes)
        {
            await _context.AddRangeAsync(bookingTimes);
        }

        public async Task<Calendar?> GetCalendarByDoctorId(Guid doctorId)
        {
            return await _entity
                .Include(x => x.Bookings)
                .SingleOrDefaultAsync(x => x.DoctorId == doctorId);
        }
    }
}
