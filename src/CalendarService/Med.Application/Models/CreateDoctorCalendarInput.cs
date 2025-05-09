namespace Med.Application.Models
{
    public class CreateDoctorCalendarInput
    {
        public required Guid DoctorId { get; set; }
        public required List<BookingTimeInput> BookingTime { get; set; }
        public required Decimal Price { get; set; }
    }
}
