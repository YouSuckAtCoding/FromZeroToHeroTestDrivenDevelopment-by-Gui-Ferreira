using Pricing.Core.Extensions;

namespace Pricing.Core
{
    public interface IPricingManager
    {
        IPricingStore _pricingStore { get; }

        Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken token);
    }
}