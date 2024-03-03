using FluentAssertions;
using NSubstitute;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;
using Pricing.Core.Tests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Pricing.Core.Tests
{
    public class ApplyPricingSpecification
    {

        [Fact]
        public async Task Should_Throw_ArgumentNullException_If_Request_Is_Null()
        {

            var _pricingManager = new PricingManager(new DummyPricingStore());
            var handleRequest = () => _pricingManager.HandleAsync(null, default);

            await handleRequest.Should().ThrowAsync<ArgumentNullException>();

        }
        [Fact]
        public async Task Should_Return_True_If_Succeded()
        {
            var _pricingManager = new PricingManager(new StubSuccessPricingStore());
            var handleRequest = await _pricingManager.HandleAsync(CreateRequest(), default);

            handleRequest.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Return_False_If_Fails_To_Save()
        {
            var _pricingManager = new PricingManager(new StubFailurePricingStore());
            var handleRequest = await _pricingManager.HandleAsync(CreateRequest(), default);

            handleRequest.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Save_Only_Once()
        {
            var spy = new SpyPricingStore();
            var _pricingManager = new PricingManager(spy);
            _ = await _pricingManager.HandleAsync(CreateRequest(), default);
            spy.NumberOfSaves.Should().Be(1);
        }
        
        [Fact]
        public async Task Should_Save_Expected_Data()
        {

            var pricingTable = new PricingTable(new[] { new PriceTier(24, 1) });
            var mockPricingStore = new MockPricingStore();
            mockPricingStore.ExpectedToSave(pricingTable);
            var _pricingManager = new PricingManager(mockPricingStore);
            _ = await _pricingManager.HandleAsync(CreateRequest(), default);


            mockPricingStore.Verify();
        }
        [Fact]
        public async Task Should_Save_Expected_Data_NSubstitue()
        {

            var pricingTable = new PricingTable(new[] { new PriceTier(24, 1) });
            var mockPricingStore = Substitute.For<IPricingStore>();
            var _pricingManager = new PricingManager(mockPricingStore);
            _ = await _pricingManager.HandleAsync(CreateRequest(), default);

            await mockPricingStore.Received().SaveAsync(Arg.Is<PricingTable>(table => table.Tiers.Count == pricingTable.Tiers.Count), default);
            
        }

        [Fact]
        public async Task Should_Save_Equivalent_Data_To_StorageAsync()
        {
            var inMemoryPricingStore = new InMemoryPricingStore();
            var pricingRequest = CreateRequest();
            var _pricingManager = new PricingManager(inMemoryPricingStore);

            _ = await _pricingManager.HandleAsync(pricingRequest, default);

            inMemoryPricingStore.GetPricingTable().Should().BeEquivalentTo(pricingRequest);


        }

        private static ApplyPricingRequest CreateRequest()
        {
            return new ApplyPricingRequest(
                    new[] { new PriceTierRequest(24, 1) }
                );
        }
    }
}
