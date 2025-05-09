namespace Med.Application.Models
{
    public class CancelAppointmentInput
    {
        public required Guid AppointmentId { get; set; }

        public string? ReasonForCanceling { get; set; }

    }
}
