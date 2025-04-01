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
    }
}
