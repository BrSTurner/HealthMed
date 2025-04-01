using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Med.Infrastructure.Mapping
{
    public class BookingTimeMapping : IEntityTypeConfiguration<BookingTime>
    {
        public void Configure(EntityTypeBuilder<BookingTime> builder)
        {
            builder.ToTable("BookingTime");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CalendarId).IsRequired();
                     
            builder.Property(x => x.Date).IsRequired();

            builder.Property(x => x.Status)
                .HasDefaultValue(BookingTimeStatus.Available)
                .IsRequired();

            builder.Property(x => x.ConsultDuration)
                .HasDefaultValue(TimeSpan.FromMinutes(30))
                .IsRequired();
        }
    }
}
