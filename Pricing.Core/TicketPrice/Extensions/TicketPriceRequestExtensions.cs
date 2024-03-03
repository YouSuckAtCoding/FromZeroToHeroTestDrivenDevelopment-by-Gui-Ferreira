using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Core.TicketPrice.Extensions
{
    public static class TicketPriceRequestExtensions
    {
        

        public static int GetDurationInHours(this TicketPriceRequest ticketPriceRequest)
        {
            const int upperHourLimit = 59;

            const int oneHourInMinutes = 60;

            var ticketDuration = (int)(ticketPriceRequest.exitTime - ticketPriceRequest.entryTime).TotalMinutes;

            return (ticketDuration + upperHourLimit) / oneHourInMinutes;
        }
    }
}
