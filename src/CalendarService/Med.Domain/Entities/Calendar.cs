using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class Calendar : Entity, IAggregateRoot
    {
        public required Guid DoctorId { get; set; }
        public virtual ICollection<BookingTime>? Bookings { get; set; }
        public decimal Price { get; set; }
    }
}
