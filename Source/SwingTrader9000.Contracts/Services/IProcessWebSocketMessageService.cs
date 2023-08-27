using CryptoProvider.Contracts.Models.WebSocket;

namespace SwingTrader9000.Contracts.Services
{
    public interface IProcessWebSocketMessageService
    {
        void ProcessWelcomeMessage();
        void ProcessCurrentPrice(CurrentPrice currentPrice);
    }
}
