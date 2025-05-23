﻿using Med.Domain.Enumerations;
using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class BookingTime : Entity, IAggregateRoot
    {
        public required Guid CalendarId { get; set; }
        public Calendar? Calendar { get; set; }
        public required DateTime Date { get; set; }
        public TimeSpan ConsultDuration { get; set; }
        public required BookingTimeStatus Status { get; set; }

        public BookingTime()
        {
            ConsultDuration = TimeSpan.FromMinutes(30);
            Status = BookingTimeStatus.Available;
        }
    }
}
