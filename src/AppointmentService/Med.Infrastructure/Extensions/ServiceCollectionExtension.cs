﻿using Med.Domain.Repositories;
using Med.Infrastructure.Data;
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
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var dockerSqlConnectionString = configuration["SQL_CONNECTION_STRING"];

            if (!string.IsNullOrEmpty(dockerSqlConnectionString))
            {
                services.AddDbContext<AppointmentContext>(c => c.UseSqlServer(configuration["SQL_CONNECTION_STRING"]));
                return;
            }

            if (useInMemory)
                services.AddDbContext<AppointmentContext>(c => c.UseInMemoryDatabase("Appointments"));
            else
                services.AddDbContext<AppointmentContext>(c => c.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
