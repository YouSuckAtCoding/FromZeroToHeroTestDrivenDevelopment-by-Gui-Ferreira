using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Pricing.Api.TicketPrice;
using Pricing.Core.TicketPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Api.Tests
{
    public class TicketPriceEndpointSpecification
    {
        [Fact]
        public async Task Should_Return_200_With_Price_When_Get_Price()
        {
            var exitTime = DateTimeOffset.UtcNow;
            var entryTime = exitTime.AddMinutes(-30);
            ITicketPriceService ticketPriceService = Substitute.For<ITicketPriceService>();

            var ticketPriceRequest = new TicketPriceRequest(entryTime, exitTime);
            
            ticketPriceService.HandleAsync(ticketPriceRequest, default).Returns(new TicketPriceResponse(2));

            var result = await TicketPriceEndpoint.HandleAsync(entryTime, exitTime, ticketPriceService, default);

            result.Should().BeOfType<Ok<TicketPriceResponse>>();
            result.Value.Price.Should().Be(2);
            await ticketPriceService.Received(1).HandleAsync(new TicketPriceRequest(entryTime, exitTime), default);

        }
    }
}
