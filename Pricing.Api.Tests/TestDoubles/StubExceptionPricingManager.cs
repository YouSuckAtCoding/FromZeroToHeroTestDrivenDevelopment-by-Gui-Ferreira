using Pricing.Core.Extensions;
using Pricing.Core;
using Pricing.Core.Domain.Exceptions;

namespace Pricing.Api.Tests.TestDoubles
{
    public class StubExceptionPricingManager : IPricingManager
    {
        public IPricingStore _pricingStore => throw new NotImplementedException();

        public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken token)
        {
            throw new InvalidPricingTierException("");
        }
    }
}
