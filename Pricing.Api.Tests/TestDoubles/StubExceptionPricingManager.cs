using Pricing.Core.Domain.Exceptions;
using Pricing.Core.ApplyPricing;

namespace Pricing.Api.Tests.TestDoubles
{
    public class StubExceptionPricingManager : IPricingManager
    {
        public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken token)
        {
            throw new InvalidPricingTierException("");
        }
    }
}
