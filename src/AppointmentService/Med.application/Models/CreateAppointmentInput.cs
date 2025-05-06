namespace Med.Application.Models
{
    public class CreateAppointmentInput
    {
        public required Guid DoctorId { get; set; }
        public required Guid PatientId { get; set; }
        public required DateTime Date { get; set; }
    }
}
