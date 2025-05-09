using Med.Domain.Entities;

namespace Med.Application.Models
{
    public class UpdateDoctorCalendarInput
    {
        public Guid DoctorId { get; set; }
        public decimal? Price { get; set; }
        public List<BookingTime> Bookings { get; set; } = [];
    }
}
