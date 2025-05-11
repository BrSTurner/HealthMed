using Med.Domain.Entites;
using Med.SharedKernel.Encryptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Med.Infrastructure.Mapping
{
    public class PatientMapping : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.CreatedAt);

            builder.OwnsOne(x => x.CPF, cpfBuilder =>
            {
                cpfBuilder.Property(e => e.Number)
                    .HasColumnName("CPF")
                    .HasConversion(
                        v => DataEncryptor.Encrypt(v),
                        v => DataEncryptor.Decrypt(v)
                    )
                    .IsRequired();
            });

            builder.OwnsOne(x => x.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Address)
                    .HasColumnName("Email")
                    .IsRequired();
            });
        }
    }
}
