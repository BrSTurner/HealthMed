using MassTransit;
using Med.Application.Consumers;
using Med.Application.Interfaces.Services;
using Med.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Med.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<AuthConsumer>();

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
