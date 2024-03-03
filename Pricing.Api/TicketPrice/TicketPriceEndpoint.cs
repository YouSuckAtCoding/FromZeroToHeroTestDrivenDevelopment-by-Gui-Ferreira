
using Microsoft.AspNetCore.Http.HttpResults;
using Pricing.Core.TicketPrice;

namespace Pricing.Api.TicketPrice
{
    public class TicketPriceEndpoint
    {
        public static async Task<Ok<TicketPriceResponse>> HandleAsync(DateTimeOffset entryTime, DateTimeOffset exitTime,
            ITicketPriceService ticketPriceService, CancellationToken token)
        {
            var result = await ticketPriceService.HandleAsync(new TicketPriceRequest(entryTime, exitTime), token);
            return TypedResults.Ok(result);
        }
    }
}
