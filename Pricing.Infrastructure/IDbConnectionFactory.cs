using System.Data;

namespace Pricing.Infrastructure.Tests
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}
