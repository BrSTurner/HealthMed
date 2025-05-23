﻿using Med.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Med.Infrastructure.Mapping
{
    public class AppointmentMapping : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.Property(x => x.CreatedAt);

            builder.ToTable("Appointments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DoctorId).IsRequired();
            builder.Property(x => x.PatientId).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ReasonForCanceling).IsRequired(false);
        }
    }
}
