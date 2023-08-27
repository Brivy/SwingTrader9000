using CryptoProvider.Contracts.Constants;
using CryptoProvider.Contracts.WebSocket;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Models.WebSocket;
using CryptoProvider.KuCoin.Queues;

namespace CryptoProvider.KuCoin.WebSocket
{
    public class PrivateWebSocket : IPrivateWebSocket
    {
        private readonly ConcurrentMessageQueue _concurrentMessageQueue;

        public PrivateWebSocket(ConcurrentMessageQueue concurrentMessageQueue)
        {
            _concurrentMessageQueue = concurrentMessageQueue;
        }

        public void SubscribeToTradeOrders()
        {
            var subscription = CreatePrivateSubscriptionMessage(RequestMessageType.Subscribe, Topic.TradeOrders);
            _concurrentMessageQueue.Enqueue(subscription);
        }

        private static WebSocketSubscriptionMessage CreatePrivateSubscriptionMessage(string type, string topic)
        {
            return new WebSocketSubscriptionMessage
            {
                Id = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                Type = type,
                Topic = topic,
                PrivateChannel = true,
                Response = true
            };
        }
    }
}
