using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles
{
    internal class SpyPricingStore : IPricingStore
    {
        public int NumberOfSaves { get; private set; }

        public Task<bool> SaveAsync(PricingTable pricintTable, CancellationToken cancellationToken)
        {
            NumberOfSaves++;
            return Task.FromResult(true);
        }
    }
}