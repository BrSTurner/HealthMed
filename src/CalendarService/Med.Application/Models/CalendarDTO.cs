using Med.Domain.Entities;

namespace Med.Application.Models
{
    public class CalendarDTO
    {
        public ICollection<BookingTime> Bookings { get; set; } = [];
        public decimal Price { get; set; }
    }
}
