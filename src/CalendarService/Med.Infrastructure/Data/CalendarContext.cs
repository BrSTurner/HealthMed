using Med.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Med.Infrastructure.Data
{
    public class CalendarContext(DbContextOptions options) : DbContext(options)
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

            modelBuilder.ApplyConfiguration(new CalendarMapping());
            modelBuilder.ApplyConfiguration(new BookingTimeMapping());            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("Calendars");

            }
        }
    }
}
