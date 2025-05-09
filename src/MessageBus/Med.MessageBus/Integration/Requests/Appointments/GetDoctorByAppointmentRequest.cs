using Med.SharedKernel.DomainObjects;

namespace Med.MessageBus.Integration.Requests.Appointments
{
    public class GetDoctorByAppointmentRequest
    {
        public CRM Crm { get; set; }
    }
}
