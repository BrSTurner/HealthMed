using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Med.Migrator
{
    public static class DatabaseMigrator
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, bool inMemoryDatabase = false) where TContext : DbContext
        {
            if (inMemoryDatabase)
                return host;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation("Applying database migrations...");
                    context.Database.Migrate();
                    logger.LogInformation("Database migration completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                    throw;
                }
            }

            return host;
        }
    }
}
