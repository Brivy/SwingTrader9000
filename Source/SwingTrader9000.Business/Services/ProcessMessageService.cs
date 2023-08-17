using Microsoft.Extensions.Logging;
using SwingTrader9000.Business.Constants;
using SwingTrader9000.Business.Queues;
using SwingTrader9000.Contracts.Models;
using SwingTrader9000.Contracts.Services;
using System.Text.Json;

namespace SwingTrader9000.Business.Services
{
    public class ProcessMessageService : IProcessMessageService
    {
        private readonly ISubscriptionMessageService _subscriptionMessageService;
        private readonly ConcurrentMessageQueue _concurrentMessageQueue;
        private readonly ILogger<ProcessMessageService> _logger;

        public ProcessMessageService(
            ISubscriptionMessageService subscriptionMessageService,
            ConcurrentMessageQueue concurrentMessageQueue,
            ILogger<ProcessMessageService> logger)
        {
            _subscriptionMessageService = subscriptionMessageService;
            _concurrentMessageQueue = concurrentMessageQueue;
            _logger = logger;
        }

        public void ProcessWelcomeMessage()
        {
            var symbols = new List<string> { Symbol.BTCUSDT };
            var subscription = _subscriptionMessageService.CreateSymbolTickerSubscriptionMessage(symbols);
            var serializeOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedSubscription = JsonSerializer.Serialize(subscription, serializeOptions);

            _concurrentMessageQueue.MessageQueue.Enqueue(serializedSubscription);
            _concurrentMessageQueue.MessageAvailable.Set();
        }

        public void ProcessSymbolTickerMessage(string message)
        {
            var symbolTicker = JsonSerializer.Deserialize<SymbolTickerMessage>(message);
            if (symbolTicker == null)
            {
                _logger.LogError("SymbolTicker message could not be deserialized: {message}", message);
                return;
            }

            _logger.LogInformation("{topic}: SymbolTicker received: {price}", symbolTicker.Topic, symbolTicker.Data.Price);
        }
    }
}
