using FluentAssertions;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles
{
    internal class MockPricingStore : IPricingStore
    {
        public PricingTable _expectedPricingTable;
        private PricingTable _savedPricingTable;
        public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
        {
            _savedPricingTable = pricingTable;
            return Task.FromResult(true);
        }

        internal void ExpectedToSave(PricingTable expectedPricingTable)
        {
            _expectedPricingTable = expectedPricingTable;
        }

        internal void Verify()
        {
            _savedPricingTable.Should().BeEquivalentTo(_expectedPricingTable);
        }
    }
}
