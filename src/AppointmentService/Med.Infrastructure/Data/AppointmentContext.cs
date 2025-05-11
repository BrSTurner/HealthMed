using Med.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Med.Infrastructure.Data
{
    public class AppointmentContext(DbContextOptions options) : DbContext(options)
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

            modelBuilder.ApplyConfiguration(new AppointmentMapping());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("Appointments");
            }
        }
    }
}
