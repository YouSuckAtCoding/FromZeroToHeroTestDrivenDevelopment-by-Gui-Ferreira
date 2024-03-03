using Dapper;
using Newtonsoft.Json;
using Pricing.Core.Domain;
using Pricing.Core.TicketPrice;
namespace Pricing.Infrastructure
{
    public class PostgresReadPricingStore : IReadPricingStore
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PostgresReadPricingStore(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
        }

        public async Task<PricingTable> GetAsync(CancellationToken token)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var res = await connection.QueryFirstAsync<string>(
                "SELECT document from Pricing");
            return JsonConvert.DeserializeObject<PricingTable>(res)!;

        }
    }
}
