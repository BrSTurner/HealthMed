﻿using Med.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Med.Infrastructure.Mapping
{
    public class SpecialityMapping : IEntityTypeConfiguration<Speciality>
    {
        public void Configure(EntityTypeBuilder<Speciality> builder)
        {

            builder.Property(x => x.CreatedAt);

            builder.ToTable("Specialities");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();
        }
    }
}
