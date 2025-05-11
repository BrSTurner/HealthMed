using Med.Domain.Enumerations;

namespace Med.Application.Models
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public required AppointmentStatus Status { get; set; }
        public required DateTime Date { get; set; }
    }
}
