using MassTransit;
using Med.Application.Consumers;
using Med.Domain.Entities;
using Med.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace Med.Integration.Tests.Base
{
    public class WebClientFixture : IDisposable
    {
        public HttpClient Client { get; private set; }
        public IServiceScopeFactory? ScopeFactory { get; private set; }

        private readonly WebApplicationFactory<AuthProgram> _factory;
        private readonly SqliteConnection _sqliteConnection;

        public WebClientFixture()
        {
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");
            _sqliteConnection.Open();

            _factory = new WebApplicationFactory<AuthProgram>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    RemoveSQLServerProvider(services);
                    RegisterDatabase(services);
                    RegisterMassTransit(services);

                    var sp = services.BuildServiceProvider();
                    ScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AuthContext>();
                    db.Database.EnsureCreated();
                });
            });

            Client = _factory.CreateClient();
        }

        private static void RemoveSQLServerProvider(IServiceCollection services)
        {
            var descriptors = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(DbContextOptions<AuthContext>) ||
                    d.ServiceType == typeof(AuthContext) ||
                    d.ImplementationType?.FullName?.Contains("SqlServer") == true ||
                    d.ImplementationType?.FullName?.Contains("Sqlite") == true ||   
                    d.ServiceType.FullName?.StartsWith("Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration") == true
                )
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

        }

        private void RegisterDatabase(IServiceCollection services)
        {
            services.AddSingleton<DbConnection>(_ => _sqliteConnection);
            services.AddDbContext<AuthContext>(options =>
            {
                options.UseSqlite(_sqliteConnection);
            });
        }

        public async Task<User?> InsertUser(User user)
        {
            if (user == null)
                return null;

            if (ScopeFactory == null)
                return null;

            using (var scope = ScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AuthContext>();
                await db.AddAsync(user);
                await db.SaveChangesAsync();
            }

            return user;
        }

        private static void RegisterMassTransit(IServiceCollection services)
        {
            var massTransitDescriptors = services
                .Where(d => d.ImplementationType?.Namespace?.Contains("MassTransit") == true)
                .ToList();

            foreach (var descriptor in massTransitDescriptors)
                services.Remove(descriptor);

            services.AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<AuthConsumer>();
            });
        }

        public void Dispose()
        {
            _sqliteConnection.Close();
            _factory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
