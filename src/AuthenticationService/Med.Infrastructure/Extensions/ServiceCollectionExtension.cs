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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            if (useInMemory)
                services.AddDbContext<AuthContext>(c => c.UseInMemoryDatabase("Authentication"));
            else
                services.AddDbContext<AuthContext>(c => c.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
