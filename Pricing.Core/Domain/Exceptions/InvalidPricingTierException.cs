using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Core.Domain.Exceptions
{
    public class InvalidPricingTierException : Exception
    {
        public InvalidPricingTierException(string? message) : base(message)
        {
        }
    }
}
