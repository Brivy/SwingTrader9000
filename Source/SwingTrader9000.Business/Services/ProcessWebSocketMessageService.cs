using CryptoProvider.Contracts.Models.WebSocket;
using CryptoProvider.Contracts.Services;
using Microsoft.Extensions.Logging;
using SwingTrader9000.Business.Constants;
using SwingTrader9000.Contracts.Services;

namespace SwingTrader9000.Business.Services
{
    public class ProcessWebSocketMessageService : IProcessWebSocketMessageService
    {
        private readonly ISubscriptionMessageService _subscriptionMessageService;
        private readonly IKuCoinWebSocketService _kuCoinWebSocketService;
        private readonly ILogger<ProcessWebSocketMessageService> _logger;

        public ProcessWebSocketMessageService(
            ISubscriptionMessageService subscriptionMessageService,
            IKuCoinWebSocketService kuCoinWebSocketService,
            ILogger<ProcessWebSocketMessageService> logger)
        {
            _subscriptionMessageService = subscriptionMessageService;
            _kuCoinWebSocketService = kuCoinWebSocketService;
            _logger = logger;
        }

        public void ProcessWelcomeMessage()
        {
            var symbols = new List<string> { Symbol.BTCUSDT };
            var subscription = _subscriptionMessageService.CreateSymbolTickerSubscriptionMessage(symbols);
            _kuCoinWebSocketService.EnqueueWebSocketMessage(subscription);
        }

        public void ProcessCurrentPrice(CurrentPrice currentPrice)
        {
            _logger.LogInformation("Current price: {price}", currentPrice.Price);
        }
    }
}
