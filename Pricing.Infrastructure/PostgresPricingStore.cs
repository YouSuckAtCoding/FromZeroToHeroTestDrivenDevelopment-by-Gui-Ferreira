using Pricing.Core;
using Pricing.Core.Domain;
using Dapper;
using System.Text;
using System.Text.Json;

namespace Pricing.Infrastructure.Tests
{
    public class PostgresPricingStore : IPricingStore
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PostgresPricingStore(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            
        }

        public async Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
        {
            var data = new DbRecord(JsonSerializer.Serialize(pricingTable));
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var res = await connection.ExecuteAsync(
                "INSERT INTO Pricing(key, document) VALUES (@key, @document)" +
                "ON CONFLICT (key) DO UPDATE SET document = excluded.document;",
                data);
            return res > 0;
        }

        private record DbRecord(string Document, string key = "ACTIVE");
    }
}
