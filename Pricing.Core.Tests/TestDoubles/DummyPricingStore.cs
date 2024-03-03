using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles
{
    public class DummyPricingStore : IPricingStore
    {
        //Dummies will never be used, theywill only provide a class for compilation
        public Task<bool> SaveAsync(PricingTable pricintTable, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
