using CryptoProvider.Contracts.Models.WebSocket;
using CryptoProvider.Contracts.WebSocket;
using Microsoft.Extensions.Logging;
using SwingTrader9000.Contracts.Services;

namespace SwingTrader9000.Business.Services
{
    public class ProcessWebSocketMessageService : IProcessWebSocketMessageService
    {
        private readonly IPrivateWebSocket _privateWebSocket;
        private readonly ILogger<ProcessWebSocketMessageService> _logger;

        public ProcessWebSocketMessageService(
            IPrivateWebSocket privateWebSocket,
            ILogger<ProcessWebSocketMessageService> logger)
        {
            _privateWebSocket = privateWebSocket;
            _logger = logger;
        }

        public void ProcessWelcomeMessage()
        {
            _privateWebSocket.SubscribeToTradeOrders();
        }

        public void ProcessCurrentPrice(CurrentPrice currentPrice)
        {
            _logger.LogInformation("Current price: {price}", currentPrice.Price);
        }
    }
}
