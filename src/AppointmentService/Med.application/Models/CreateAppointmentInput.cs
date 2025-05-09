using Med.SharedKernel.DomainObjects;

namespace Med.Application.Models
{
    public class CreateAppointmentInput
    {
        public required string Crm { get; set; }
        public required Guid PatientId { get; set; }
        public required DateTime Date { get; set; }
    }
}
