using CryptoProvider.Contracts.Models.Api;

namespace CryptoProvider.Contracts.Clients
{
    public interface IOrderClient
    {
        Task<Order> CreateLimitOrderAsync(LimitOrderRequest limitOrderRequest, CancellationToken cancellationToken = default);
    }
}
