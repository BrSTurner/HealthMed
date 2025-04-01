using Med.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Med.Infrastructure.Mapping
{
    public class DoctorMapping : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            
            builder.HasOne(x => x.Speciality)
                .WithMany(x => x.Doctors)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.SpecialityId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.OwnsOne(x => x.CRM, crmBuilder =>
            {
                crmBuilder.Property(e => e.Number)
                    .HasColumnName("CRM")
                    .IsRequired();
            });
        }
    }
}
