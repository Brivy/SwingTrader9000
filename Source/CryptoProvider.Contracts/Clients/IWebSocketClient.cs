using CryptoProvider.Contracts.Models.Api;

namespace CryptoProvider.Contracts.Clients
{
    public interface IWebSocketClient
    {
        Task<InitialWebSocketData> GetPrivateWebSocketDataAsync(CancellationToken cancellationToken = default);
    }
}
