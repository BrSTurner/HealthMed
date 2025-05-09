namespace Med.Application.Models
{
    public class BookingTimeInput
    {
        public required Guid CalendarId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ConsultDuration { get; set; }
    }
}
