using FluentAssertions;
using Pricing.Core.Domain;
using Pricing.Core.Domain.Exceptions;

namespace Pricing.Core.Tests.Domain
{
    public class PricingTierSpecification
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(25)]
        public void Should_Throw_InvalidPricingTierException_When_Hour_Is_Invalid(int hour)
        {
            var create = () => new PriceTier(hour, 1);
            create.Should().ThrowExactly<InvalidPricingTierException>()
                .WithMessage("Invalid hour limit");
        }
        [Fact]
        public void Should_Throw_InvalidPricingTierException_When_Price_Is_Negative()
        {
            var create = () => new PriceTier(24, -5);
            create.Should().ThrowExactly<InvalidPricingTierException>("Invalid price");
        }
    }
}
