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

        public async Task<Calendar?> GetCalendarByDoctor(Guid doctorId)
        {
            return await _entity.FindAsync(doctorId);
        }

        public async void CreateDoctorCalendar(Calendar calendar)
        {
            await _entity.AddAsync(calendar);
        }

        public void CreateBookingTime(List<BookingTime> bookingTimes)
        {
            _context.AddRangeAsync(bookingTimes);
        }
    }
}
