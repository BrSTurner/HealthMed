namespace Med.MessageBus.Integration.Requests.Calendars
{
    public class UpdateCalendarIfAvailableRequest
    {
        public DateTime Date { get; set; }
        public Guid DoctorId { get; set; }
    }
}
