using CryptoProvider.Contracts.Models;

namespace CryptoProvider.Contracts.Clients
{
    public interface ICryptoClient
    {
        Task<InitialWebSocketData> GetInitialWebSocketDataAsync(CancellationToken cancellationToken = default);
        Task<CurrentPrice> GetTickerAsync(string ticker, CancellationToken cancellationToken = default);
    }
}