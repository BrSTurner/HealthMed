namespace Med.Application.Models
{
    public class CalendarDTO
    {
        public Guid Id { get; set; }
        public ICollection<BookingTimeDto> Bookings { get; set; } = [];
        public decimal Price { get; set; }
    }
}
