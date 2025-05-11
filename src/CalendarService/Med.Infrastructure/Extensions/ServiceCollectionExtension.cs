using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Med.Infrastructure.Repositories;
using Med.Infrastructure.UoW;
using Med.SharedKernel.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Med.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool useInMemory = false)
        {
            services.AddScoped<ICalendarRepository, CalendarRepository>();
            services.AddScoped<IBookingTimeRepository, BookingTimeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            if(useInMemory)
                services.AddDbContext<CalendarContext>(c => c.UseInMemoryDatabase("Calendars"));
            //else
            //    services.AddDbContext<CalendarContext>(c => c.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
