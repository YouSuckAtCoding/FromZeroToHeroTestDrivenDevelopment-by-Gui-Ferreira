using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles
{
    public class InMemoryPricingStore : IPricingStore
    {
        private PricingTable PricingTable;
        public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
        {
            PricingTable = pricingTable;
            return Task.FromResult(true);
        }

        public PricingTable GetPricingTable()
        {
            return PricingTable;
        }
    }
}
