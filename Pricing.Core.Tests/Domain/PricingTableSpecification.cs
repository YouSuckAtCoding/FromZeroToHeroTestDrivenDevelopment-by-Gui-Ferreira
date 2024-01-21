using FluentAssertions;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests.Domain
{
    public class PricingTableSpecification
    {
        [Fact]
        public void Should_Throw_ArgumentNullException_If_Price_Tiers_Are_Null()
        {
            var create = () => new PricingTable(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ArgumentException_If_Has_No_Price_Tiers()
        {
            var create = () => new PricingTable(Array.Empty<PriceTier>());
            create.Should().ThrowExactly<ArgumentException>()
                .WithParameterName(nameof(PriceTier))
                .WithMessage("Missing pricing tier*");
        }

        [Fact]
        public void Should_Have_One_Tier_When_Created_With_One()
        {
            var pricingTable = new PricingTable(new[] { CreatePriceTier(24) });
            pricingTable.Tiers.Should().HaveCount(1);

        }

        [Fact]
        public void Price_Tiers_Should_Be_Ordered_By_Hour_Limit()
        {
            var pricingTable = new PricingTable(new[] { CreatePriceTier(24), CreatePriceTier(4) });
            pricingTable.Tiers.Should().BeInAscendingOrder(tier => tier.HourLimit);

        }

        [Theory]
        [InlineData(2, 1, 25)]
        [InlineData(3, 2, 49)]
        public void Maximum_Daily_Price_Should_Be_Calculated_Using_Tiers_If_Not_Defined(decimal price1, decimal price2, decimal maxPrice)
        {
            var pricingTable = new PricingTable(new[] { CreatePriceTier(1, price1), CreatePriceTier(24, price2) }, maximumDailyPrice: null);
            pricingTable.GetMaxDailyPrice().Should().Be(maxPrice);
        }

        [Fact]
        public void Should_Be_Able_To_Set_Maximum_Daily_Price()
        {
            const int maxDailyPrice = 15;
            var pricingTable = new PricingTable(new[] { CreatePriceTier(24, 1) }, maximumDailyPrice: maxDailyPrice);

            pricingTable.GetMaxDailyPrice().Should().Be(maxDailyPrice);
        }

        [Fact]
        public void Should_Fail_If_Price_Tiers_Do_Not_Cover_24_Hours()
        {
            var create = () => new PricingTable(new[] { CreatePriceTier(20) });
            create.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Fail_If_Max_Daily_Price_Greater_Than_Tiers_Price()
        {
            var create = () => new PricingTable(new[] { CreatePriceTier(24, 1) }, maximumDailyPrice: 26);
            create.Should().Throw<ArgumentException>();

        }

        [Fact]
        public void Should_Calculate_Price_Per_Hour()
        {

        }


        private static PriceTier CreatePriceTier(int hourLimit = 24, decimal price = 1) => new PriceTier(hourLimit, price);
    }
}