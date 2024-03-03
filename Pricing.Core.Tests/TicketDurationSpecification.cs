using FluentAssertions;
using Pricing.Core.TicketPrice;
using Pricing.Core.TicketPrice.Extensions;

namespace Pricing.Core.Tests
{
    public class TicketDurationSpecification
    {
        [Fact]
        public void Should_Be_1_When_Time_Span_Is_30_Min()
        {
            var entry = DateTimeOffset.UtcNow;
            var request = new TicketPriceRequest(
                entry, entry.AddMinutes(30)
                );

            request.GetDurationInHours().Should().Be(1);
        }

        [Fact]
        public void Should_Be_2_When_Time_Span_Is_2_Min()
        {
            var entry = DateTimeOffset.UtcNow;
            var request = new TicketPriceRequest(
                entry, entry.AddHours(2)
                );

            request.GetDurationInHours().Should().Be(2);
        }
    }
}
