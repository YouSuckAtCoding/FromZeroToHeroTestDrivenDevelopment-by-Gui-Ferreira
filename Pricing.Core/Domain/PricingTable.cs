using System.Collections.Immutable;

namespace Pricing.Core.Domain
{
    public class PricingTable
    {
        public IReadOnlyCollection<PriceTier> Tiers { get; }
        public decimal? MaximumDailyPrice { get; }

        public PricingTable(IEnumerable<PriceTier> tiers, decimal? maximumDailyPrice = null)
        {
            MaximumDailyPrice = maximumDailyPrice;

            Tiers = tiers?.OrderBy(tier => tier.HourLimit).ToImmutableArray() ?? throw new ArgumentNullException();

            if (!Tiers.Any())
            {
                throw new ArgumentException("Missing pricing tier",nameof(PriceTier));
            }

            if(Tiers.Last().HourLimit != 24) throw new ArgumentException();

            if(MaximumDailyPrice.HasValue && MaximumDailyPrice.Value > CalculateMaximumDailyPrice())
                throw new ArgumentException();

        }

        public decimal GetMaxDailyPrice()
        {
            if (MaximumDailyPrice.HasValue) return MaximumDailyPrice.Value;

            decimal total = CalculateMaximumDailyPrice();

            return total;
        }

        private decimal CalculateMaximumDailyPrice()
        {
            decimal total = 0;
            var hoursIncluded = 0;

            foreach (var tier in Tiers)
            {
                total += tier.Price * (tier.HourLimit - hoursIncluded);
                hoursIncluded = tier.HourLimit;
            }

            return total;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Tiers.Select(tier => tier.ToString()));
        }
    }
}