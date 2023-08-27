using CryptoProvider.Contracts.Models.Api;

namespace CryptoProvider.Contracts.Clients
{
    public interface ICryptoClient
    {
        Task<IEnumerable<AccountData>> GetAccountsAsync(CancellationToken cancellationToken = default);
        Task<InitialWebSocketData> GetPublicWebSocketDataAsync(CancellationToken cancellationToken = default);
        Task<InitialWebSocketData> GetPrivateWebSocketDataAsync(CancellationToken cancellationToken = default);
        Task<CurrentPrice> GetTickerAsync(string ticker, CancellationToken cancellationToken = default);
    }
}
