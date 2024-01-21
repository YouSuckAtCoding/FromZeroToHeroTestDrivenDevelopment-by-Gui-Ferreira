using Pricing.Core.Domain.Exceptions;

namespace Pricing.Core.Domain
{
    public class PriceTier
    {
        public int HourLimit { get; }
        public decimal Price { get; set; }

        public PriceTier(int hourLimit, decimal price)
        {
            if (hourLimit < 1 || hourLimit > 24) throw new InvalidPricingTierException("Invalid hour limit");   

            if(price < 0) throw new InvalidPricingTierException("Invalid price");

            HourLimit = hourLimit;
            Price = price;
        }

       
    }
}