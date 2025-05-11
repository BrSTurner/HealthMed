using MassTransit;
using Med.application.Consumers;
using Med.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Med.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAppointmentService, AppointmentService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<AppointmentConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RABBITMQ_HOST"], config =>
                    {
                        config.Username(configuration["RABBITMQ_USER"] ?? string.Empty);
                        config.Password(configuration["RABBITMQ_PASSWORD"] ?? string.Empty);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
