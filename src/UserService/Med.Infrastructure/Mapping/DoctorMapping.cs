using Med.Domain.Entites;
using Med.SharedKernel.Encryptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            builder.Property(x => x.CreatedAt);
            
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
                    .HasConversion(
                        v => DataEncryptor.Encrypt(v),
                        v => DataEncryptor.Decrypt(v) 
                    )
                    .IsRequired();
            });
        }
    }
}
