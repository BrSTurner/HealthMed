using Med.Domain.Enumerations;

namespace Med.Application.Models
{
    public class ReplyAppointmentInput
    {
        public required Guid AppointmentId { get; set; }
        public required AppointmentStatus Status { get; set; }
    }
}
