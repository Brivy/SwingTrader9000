using CryptoProvider.Contracts.Models;

namespace CryptoProvider.Contracts.Clients
{
    public interface ICryptoClient
    {
        Task<InitialWebSocketData> GetInitialWebSocketDataAsync(CancellationToken cancellationToken);
    }
}