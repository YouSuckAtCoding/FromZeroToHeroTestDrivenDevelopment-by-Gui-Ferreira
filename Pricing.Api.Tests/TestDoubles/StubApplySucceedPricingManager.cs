using Pricing.Core.ApplyPricing;

namespace Pricing.Api.Tests.TestDoubles
{
    public class StubApplySucceedPricingManager : IPricingManager
    {
        public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken token)
        {
            return Task.FromResult(true);
        }
    }
}
