using Med.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Med.Infrastructure.Mapping
{
    public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.Property(x => x.CreatedAt);
            builder.ToTable("UserRoles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);

            builder.HasOne(x => x.User)
               .WithMany(x => x.Roles)
               .HasForeignKey(x => x.UserId);
        }
    }
}
