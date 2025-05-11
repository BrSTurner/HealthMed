using Med.Domain.Enumerations;

namespace Med.Application.Models
{
    public class BookingTimeDto
    {
        public Guid Id { get; set; }
        public required DateTime Date { get; set; }
        public TimeSpan ConsultDuration { get; set; }
        public required BookingTimeStatus Status { get; set; }
    }
}
