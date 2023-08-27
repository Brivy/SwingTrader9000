using CryptoProvider.Contracts.Interfaces;

namespace CryptoProvider.Contracts.Services
{
    public interface IKuCoinWebSocketService
    {
        Task InitializeAsync(Func<IWebSocketMessage, bool> callback, Func<IWebSocketMessage, CancellationToken, Task<bool>> asyncCallback, CancellationToken cancellationToken = default);
        void EnqueueWebSocketMessage(object message);
    }
}
