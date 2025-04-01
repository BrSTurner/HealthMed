using Med.Domain.Enumerations;
using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class Appointment : Entity
    {
        public required Guid DoctorId { get; set; }
        public required Guid PatientId { get; set; }
        public required DateTime Date { get; set; }
        public required AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public string? ReasonForCanceling { get; set; }
    }
}
