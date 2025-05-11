using Med.Domain.Entites;
using Med.Infrastructure.Mapping;
using Med.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Med.Infrastructure.Data
{
    public class UserContext(DbContextOptions options) : DbContext(options)
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

            modelBuilder.ApplyConfiguration(new DoctorMapping());
            modelBuilder.ApplyConfiguration(new PatientMapping());
            modelBuilder.ApplyConfiguration(new SpecialityMapping());

            modelBuilder.Entity<Speciality>().HasData(SpecialitySeeding.Specialities);
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
