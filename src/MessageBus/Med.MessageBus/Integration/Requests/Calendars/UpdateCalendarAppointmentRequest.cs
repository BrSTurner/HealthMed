namespace Med.MessageBus.Integration.Requests.Calendars
{
    public class UpdateCalendarAppointmentRequest
    {
        public Guid BookingTimeId { get; set; }
        public bool? IsCancelled { get; set; }
    }
}
