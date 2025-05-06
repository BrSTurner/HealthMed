using Med.Domain.Entities;

namespace Med.Application.Models
{
    public class CreateCalendarAppointmentInput
    {
        public required Guid DoctorId { get; set; }
        public required BookingTime BookingTime { get; set; }
        public required Decimal Price { get; set; }
    }
}
