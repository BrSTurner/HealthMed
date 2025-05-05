using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Med.Infrastructure.Data
{
    public class AuthContext(DbContextOptions options) : DbContext(options)
    {
        public DbConnection DbConnection
        {
            get
            {
                return Database.GetDbConnection();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new UserRoleMapping());

            modelBuilder.Entity<Role>().HasData
            (
                new Role { Id = (int)RolesEnum.Admin, Name = "Admin", CreatedAt = new DateTime(2025, 05, 05, 00, 00, 00) },
                new Role { Id = (int)RolesEnum.Doctor, Name = "Doctor", CreatedAt = new DateTime(2025, 05, 05, 00, 00, 00) },
                new Role { Id = (int)RolesEnum.Patient, Name = "Patient", CreatedAt = new DateTime(2025, 05, 05, 00, 00, 00) }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("Users");
            }
        }
    }
}
