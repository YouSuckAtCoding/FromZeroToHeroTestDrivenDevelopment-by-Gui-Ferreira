using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Api.Tests.TestDoubles;
using Pricing.Core.Tests.TestDoubles;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain.Exceptions;
namespace Pricing.Api.Tests
{
    public class ApplyPricingEndpointSpecification
    {
        private const string RequestUri = "/PricingTable";

        [Fact]
        public async Task Should_Return_500_When_Causes_An_Exception()
        {
            using var client = CreateApiWithPricingManager<StubExceptionPricingManager>().CreateClient();

            var response = await client.PutAsJsonAsync(RequestUri, CreateRequest());

            response.Should().HaveStatusCode(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Should_Return_400_When_PricingManager_Return_False()
        {
            using var client = CreateApiWithPricingManager<StubApplyFailedPricingManager>().CreateClient();

            var response = await client.PutAsJsonAsync(RequestUri, CreateRequest());

            response.Should().HaveStatusCode(HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task Should_Return_200_When_PricingManager_Return_True()
        {
            using var client = CreateApiWithPricingManager<StubApplySucceedPricingManager>().CreateClient();

            var response = await client.PutAsJsonAsync(RequestUri, CreateRequest());

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Send_Request_To_Pricing_Manager()
        {
            var pricingStore = new InMemoryPricingStore();
            var api = new ApiFactory(services =>
            {

                services.TryAddScoped<IPricingStore>(s => pricingStore);

            });
            var client = api.CreateClient();
            var request = CreateRequest();
            var response = await client.PutAsJsonAsync(RequestUri, request);

            pricingStore.GetPricingTable().Should().BeEquivalentTo(request);
        }

        private ApiFactory CreateApiWithPricingManager<T>()
            where T : class, IPricingManager
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(IPricingManager));

                services.TryAddScoped<IPricingManager, T>();

            });
            return api;
        }
        private static ApplyPricingRequest CreateRequest()
        {
            return new ApplyPricingRequest(new[] { new PriceTierRequest(24, 1) });
        }
    }


}
