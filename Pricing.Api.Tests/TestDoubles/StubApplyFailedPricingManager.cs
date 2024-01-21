using Pricing.Core.Extensions;
using Pricing.Core;

namespace Pricing.Api.Tests.TestDoubles
{
    public class StubApplyFailedPricingManager : IPricingManager
    {
        public IPricingStore _pricingStore => throw new NotImplementedException();

        public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken token)
        {
            return Task.FromResult(false);
        }
    }
}
