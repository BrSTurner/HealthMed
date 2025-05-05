using Microsoft.Extensions.DependencyInjection;

namespace Med.MessageBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();
        }
    }
}
