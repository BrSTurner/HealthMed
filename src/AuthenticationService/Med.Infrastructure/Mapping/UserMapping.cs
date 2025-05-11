using Med.Domain.Entities;
using Med.SharedKernel.Encryptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Med.Infrastructure.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.Property(x => x.CreatedAt);
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Username)
                .HasConversion(
                        v => DataEncryptor.Encrypt(v),
                        v => DataEncryptor.Decrypt(v)
                ).IsRequired();

            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(x => x.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Address)
                    .HasColumnName("Email")
                    .IsRequired();
            });
        }
    }
}
