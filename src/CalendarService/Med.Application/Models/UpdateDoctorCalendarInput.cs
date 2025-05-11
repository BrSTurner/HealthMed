using Med.Domain.Entities;

namespace Med.Application.Models
{
    public class UpdateDoctorCalendarInput
    {
        public Guid Id { get; set; }
        public decimal? Price { get; set; }
        public List<BookingTimeInput> Bookings { get; set; } = [];
    }
}
