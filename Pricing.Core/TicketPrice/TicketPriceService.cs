using Pricing.Core.TicketPrice;

namespace Pricing.Core
{
    public class TicketPriceService : ITicketPriceService
    {
        private readonly IReadPricingStore pricingStore;
        private readonly IPriceCalculator pricingCalculator;

        public TicketPriceService(IReadPricingStore pricingStore, IPriceCalculator pricingCalculator)
        {
            this.pricingStore = pricingStore;
            this.pricingCalculator = pricingCalculator;
        }
        public async Task<TicketPriceResponse> HandleAsync(TicketPriceRequest request, CancellationToken token)
        {

            if (request.entryTime >= request.exitTime) 
                throw new ArgumentException();

            var pricingTable = await pricingStore.GetAsync(token);

            var price = pricingCalculator.Calculate(pricingTable, request);

            return new TicketPriceResponse(price);
        }
    }
}
