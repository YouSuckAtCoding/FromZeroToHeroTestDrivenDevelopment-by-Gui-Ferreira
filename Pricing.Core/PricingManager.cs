using Pricing.Core.Extensions;

namespace Pricing.Core
{
    public class PricingManager : IPricingManager
    {
        public PricingManager(IPricingStore pricingStore)
        {
            _pricingStore = pricingStore;
        }

        public IPricingStore _pricingStore { get; }

        public async Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request);
            var pricingTable = request.ToPricingTable();
            return await _pricingStore.SaveAsync(pricingTable, token);

        }
    }
}
