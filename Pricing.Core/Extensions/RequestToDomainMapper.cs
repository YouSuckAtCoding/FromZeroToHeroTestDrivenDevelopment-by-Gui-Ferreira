using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Core.Extensions
{
    public static class RequestToDomainMapper
    {
        public static PricingTable ToPricingTable(this ApplyPricingRequest request)
        {
            return new PricingTable(request.Tiers.Select(tier => new PriceTier(tier.HourLimit, tier.Price)));
        }
    }
}
