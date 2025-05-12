namespace Med.MessageBus.Integration.Requests.Appointments
{
    public class GetDoctorByAppointmentResponse
    {
        public Guid DoctorId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
