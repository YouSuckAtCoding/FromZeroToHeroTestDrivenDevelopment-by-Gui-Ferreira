using Pricing.Core.ApplyPricing;

namespace Pricing.Api.Tests.TestDoubles
{
    public class StubApplyFailedPricingManager : IPricingManager
    {
        public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken token)
        {
            return Task.FromResult(false);
        }
    }
}
