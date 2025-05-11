using Med.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Med.Infrastructure.Mapping
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.Property(x => x.CreatedAt);

            builder.ToTable("Roles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();

            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
