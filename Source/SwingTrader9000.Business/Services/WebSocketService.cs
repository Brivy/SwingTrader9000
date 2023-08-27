using CryptoProvider.Contracts.Interfaces;
using CryptoProvider.Contracts.Models.WebSocket;
using CryptoProvider.Contracts.WebSocket;
using Microsoft.Extensions.Logging;
using SwingTrader9000.Contracts.Services;

namespace SwingTrader9000.Business.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly ICryptoWebSocket _cryptoWebSocket;
        private readonly IProcessWebSocketMessageService _processWebSocketMessageService;
        private readonly ILogger<WebSocketService> _logger;

        public WebSocketService(
            ICryptoWebSocket cryptoWebSocket,
            IProcessWebSocketMessageService processWebSocketMessageService,
            ILogger<WebSocketService> logger)
        {
            _cryptoWebSocket = cryptoWebSocket;
            _processWebSocketMessageService = processWebSocketMessageService;
            _logger = logger;
        }

        public Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            return _cryptoWebSocket.InitializeAsync(OnMessageReceived, OnMessageReceivedAsync, cancellationToken);
        }

        private bool OnMessageReceived(IWebSocketMessage webSocketMessage)
        {
            switch (webSocketMessage)
            {
                case BasicMessage basicMessage:
                    _logger.LogInformation("Received message: {type}", basicMessage.Type);
                    return true;
                case WelcomeMessage welcomeMessage:
                    _logger.LogInformation("Received welcome message of type: {type}", welcomeMessage.Type);
                    _processWebSocketMessageService.ProcessWelcomeMessage();
                    return true;
                case TradeOrder tradeOrder:
                    _logger.LogInformation("Received trade order: {tradeOrder}", tradeOrder);
                    return true;
                default:
                    return false;
            }
        }

        private Task<bool> OnMessageReceivedAsync(IWebSocketMessage webSocketMessage, CancellationToken cancellationToken)
        {
            switch (webSocketMessage)
            {
                case CurrentPrice currentPrice:
                    _logger.LogInformation("Received current price: {price}", currentPrice.Price);
                    return Task.FromResult(true);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}
