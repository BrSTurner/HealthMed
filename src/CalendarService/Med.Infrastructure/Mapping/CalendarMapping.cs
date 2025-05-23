﻿using Med.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Med.Infrastructure.Mapping
{
    public class CalendarMapping : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {

            builder.Property(x => x.CreatedAt);

            builder.ToTable("Calendars");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DoctorId).IsRequired();
            builder.Property(x => x.Price).IsRequired();

            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.Calendar)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.CalendarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
