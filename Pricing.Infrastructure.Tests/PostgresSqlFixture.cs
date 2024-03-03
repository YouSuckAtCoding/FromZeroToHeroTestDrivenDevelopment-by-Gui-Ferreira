using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace Pricing.Infrastructure.Tests
{
    public class PostgresSqlFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgresSqlContainer
            = new PostgreSqlBuilder().Build();

        public IDbConnectionFactory ConnectionFactory;
        public async Task InitializeAsync()
        {
            await _postgresSqlContainer.StartAsync();
            ConnectionFactory = new NpgsqlConnectionFactory(_postgresSqlContainer.GetConnectionString());
            
            await new DatabaseInitializer(ConnectionFactory).InitializeAsync();
        }
        public Task DisposeAsync()
        {
            return _postgresSqlContainer.DisposeAsync().AsTask();
        }
       
    }
}
