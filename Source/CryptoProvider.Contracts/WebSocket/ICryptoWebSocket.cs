using CryptoProvider.Contracts.Interfaces;

namespace CryptoProvider.Contracts.WebSocket
{
    public interface ICryptoWebSocket
    {
        Task InitializeAsync(Func<IWebSocketMessage, bool> callback, Func<IWebSocketMessage, CancellationToken, Task<bool>> asyncCallback, CancellationToken cancellationToken = default);
    }
}
