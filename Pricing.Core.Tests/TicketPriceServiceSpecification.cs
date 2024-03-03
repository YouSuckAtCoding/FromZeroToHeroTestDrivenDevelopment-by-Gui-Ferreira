using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReceivedExtensions;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;
using Pricing.Core.TicketPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Core.Tests
{
    public class TicketPriceServiceSpecification
    {
        private readonly IPriceCalculator _priceCalculator;
        private readonly IReadPricingStore _pricingStore;
        private readonly TicketPriceService _service;
        public TicketPriceServiceSpecification()
        {
            _priceCalculator = Substitute.For<IPriceCalculator>();
            _pricingStore = Substitute.For<IReadPricingStore>();
            _service = new TicketPriceService(_pricingStore, _priceCalculator);
        }
        [Fact]
        public async Task Should_Get_PricingTable_From_Store()
        {
            await _service.HandleAsync(CreateRequest(), default);

            await _pricingStore.Received(1).GetAsync(default);
        }

        

        [Fact]
        public async Task Should_Get_Price_From_Calculator_Using_Pricing_Table()
        {
            var pricingTable = new PricingTable(new[] { new PriceTier(24, 1) });

            _pricingStore.GetAsync(default).Returns(pricingTable);
            
            var ticketPriceRequest = CreateRequest();

            await _service.HandleAsync(ticketPriceRequest, default);

            _priceCalculator.Received(1).Calculate(pricingTable, ticketPriceRequest);


        }

        [Fact]
        public async Task Should_Return_Price_From_Calculator()
        {
            var pricingTable = new PricingTable(new[] { new PriceTier(24, 1) });

            _pricingStore.GetAsync(default).Returns(pricingTable);

            var ticketPriceRequest = CreateRequest();

            _priceCalculator.Calculate(pricingTable, ticketPriceRequest).Returns(2);
            var result = await _service.HandleAsync(ticketPriceRequest, default);

            result.Price.Should().Be(2);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-19)]
        public async Task Should_Throw_An_ArgumentException_When_Entry_Time_Is_Eq_Or_Gt_Exit_Time(int duration)
        {
            var handle = () => _service.HandleAsync(
               CreateRequest(duration),
               default
            );
            
            await handle.Should().ThrowAsync<ArgumentException>();
        }

        private static TicketPriceRequest CreateRequest(int durationInMinutes = 5)
        {
            var entry = DateTime.Now;
            return new TicketPriceRequest(entry, entry.AddMinutes(durationInMinutes));
        }
    }
}
