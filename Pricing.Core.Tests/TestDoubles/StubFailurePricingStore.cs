using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles
{
    public class StubFailurePricingStore : IPricingStore
    {
        public Task<bool> SaveAsync(PricingTable pricintTable, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }
    }
}
