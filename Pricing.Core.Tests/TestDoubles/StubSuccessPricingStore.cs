using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles
{
    public class StubSuccessPricingStore : IPricingStore
    {
        public Task<bool> SaveAsync(PricingTable pricintTable, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
