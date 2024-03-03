using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pricing.Infrastructure.Tests
{
    public class PricingStoreSpecification : IClassFixture<PostgresSqlFixture>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public PricingStoreSpecification(PostgresSqlFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }
        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new PostgresPricingStore(null!);

            create.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async void Should_Return_True_If_Save_With_Success()
        {
            IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
            PricingTable pricingTable = CreatePricingTable();

            var res = await store.SaveAsync(pricingTable, default);

            res.Should().BeTrue();
        }


        [Fact]
        public async void Should_Insert_If_Not_Exists()
        {
            IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
            PricingTable pricingTable = CreatePricingTable();

            using IDbConnection connection = await CleanupPricingStore();

            var res = await store.SaveAsync(pricingTable, default);

            res.Should().BeTrue();
        }

        [Fact]
        public async void Should_Replace_If_Already_Exists()
        {
            IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
            

            await store.SaveAsync(CreatePricingTable(), default);

            var newPricingTable = new PricingTable(new[] { new PriceTier(24, 4) });

            await store.SaveAsync(newPricingTable, default);
            var data = await GetPricingsFromStore();

            using (new AssertionScope())
            {
                data.Should().HaveCount(1)
                .And
                .Subject
                .First().document.Equals(JsonSerializer.Serialize(newPricingTable));
            }

            
        }

        private async Task<IEnumerable<dynamic>> GetPricingsFromStore()
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var data = await connection.QueryAsync("Select * from pricing");
            return data;
        }

        private static PricingTable CreatePricingTable()
        {
            return new PricingTable(new[]
            {
                 new PriceTier(24, 1)
            });
        }


        private async Task<IDbConnection> CleanupPricingStore()
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync
                (
                    "truncate table pricing;"
                );
            return connection;
        }

    }
}
