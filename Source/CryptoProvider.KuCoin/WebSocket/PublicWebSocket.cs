using CryptoProvider.Contracts.Constants;
using CryptoProvider.Contracts.WebSocket;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Models.WebSocket;
using CryptoProvider.KuCoin.Queues;

namespace CryptoProvider.KuCoin.WebSocket
{
    public class PublicWebSocket : IPublicWebSocket
    {
        private readonly ConcurrentMessageQueue _concurrentMessageQueue;

        public PublicWebSocket(ConcurrentMessageQueue concurrentMessageQueue)
        {
            _concurrentMessageQueue = concurrentMessageQueue;
        }

        public void SubscribeToSymbolTicker(List<string> symbols)
        {
            if (symbols is null || !symbols.Any()) return;
            var topic = $"{Topic.Ticker}:{string.Join(",", symbols)}";
            var subscription = CreatePublicSubscriptionMessage(RequestMessageType.Subscribe, topic);
            _concurrentMessageQueue.Enqueue(subscription);
        }

        private static WebSocketSubscriptionMessage CreatePublicSubscriptionMessage(string type, string topic)
        {
            return new WebSocketSubscriptionMessage
            {
                Id = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                Type = type,
                Topic = topic,
                PrivateChannel = false,
                Response = true
            };
        }
    }
}
