using FluentAssertions;
using Pricing.Core.Domain;
using Pricing.Core.TicketPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Core.Tests
{
    public class PriceCalculatorSpecification
    {
        
        private readonly IPriceCalculator _priceCalculator = new PriceCalculator();

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public async Task Should_Return_1hour_Price_For_30min_Ticket(int priceFirstHour)
        {
            var exitTime = DateTimeOffset.UtcNow;
            var entryTime = exitTime.AddMinutes(-30);

            var pricingTable = new PricingTable(new[]
            {
                new PriceTier(1, priceFirstHour),
                new PriceTier(24,1)
            });

            var result = _priceCalculator.Calculate(pricingTable, new TicketPriceRequest(entryTime, exitTime));

            result.Should().Be(priceFirstHour);
        }

        [Fact]
        public async Task Should_Return_5hour_Price_For_4hour_Ticket()
        {
            var exitTime = DateTimeOffset.UtcNow;
            var entryTime = exitTime.AddHours(-4).AddMinutes(-30);

            var pricingTable = new PricingTable(new[]
            {
                new PriceTier(1, 2),
                new PriceTier(4,1),
                new PriceTier(24,1)
            });

            var result = _priceCalculator.Calculate(pricingTable, new TicketPriceRequest(entryTime, exitTime));

            result.Should().Be(6);
        }

        [Fact]
        public void Should_Be_Max_Daily_Price_If_Calculated_Price_Exceeds_It()
        {
            var exitTime = DateTimeOffset.UtcNow;
            int Expected = 10;
            var entryTime = exitTime.AddHours(-20);

            var pricingTable = new PricingTable(new[]
            {
                new PriceTier(24,1)
            }, Expected);

            var price = _priceCalculator.Calculate(pricingTable, new TicketPriceRequest(entryTime, exitTime));

            price.Should().Be(Expected);

        }
    }
}
