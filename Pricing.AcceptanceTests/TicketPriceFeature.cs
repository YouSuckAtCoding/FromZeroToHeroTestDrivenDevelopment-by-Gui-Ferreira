using FluentAssertions;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Extensions;
using Pricing.Core.TicketPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Pricing.AcceptanceTests.ApiFactory;

namespace Pricing.AcceptanceTests
{
    public class TicketPriceFeature : IClassFixture<ApiFactory>
    {

        private readonly HttpClient _client;
        public TicketPriceFeature(ApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Given_30min_Ticket_When_1Hour_Has_Price_Of_2_then_return_2()
        {
            await _client.PutAsJsonAsync("PricingTable", new ApplyPricingRequest(new[]
            {
                    new PriceTierRequest(1, 2),
                    new PriceTierRequest(24, 5)
            }));

            var exitTime = DateTimeOffset.UtcNow;
            var entryTime = exitTime.AddMinutes(-30);

            var response = await _client.GetFromJsonAsync<TicketPriceResponse>($"TicketPrice?entryTime={entryTime:u}&exitTime={exitTime:u}");

            response.Should().NotBeNull();
            response.Price.Should().Be(2);
        
        }

    }
}
