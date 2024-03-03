using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Api;
using Pricing.Infrastructure;
using Testcontainers.PostgreSql;

namespace Pricing.AcceptanceTests
{
    public partial class ApiFactory : WebApplicationFactory<IApiAssemblyMarker>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgresSqlContainer
           = new PostgreSqlBuilder().Build();

        public async Task InitializeAsync()
        {
            await _postgresSqlContainer.StartAsync();
            
            await new DatabaseInitializer(new NpgsqlConnectionFactory(_postgresSqlContainer.GetConnectionString())).InitializeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IDbConnectionFactory));
                services.TryAddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(_postgresSqlContainer.GetConnectionString()));
            });
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _postgresSqlContainer.DisposeAsync().AsTask();
        }
    }
}
