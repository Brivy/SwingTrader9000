using System.Collections.Concurrent;
using System.Text.Json;
using CryptoProvider.KuCoin.Models.WebSocket;

namespace CryptoProvider.KuCoin.Queues
{
    public class ConcurrentMessageQueue
    {
        public ConcurrentQueue<string> MessageQueue { get; } = new ConcurrentQueue<string>();
        public AutoResetEvent MessageAvailable { get; } = new AutoResetEvent(false);

        public void Enqueue(WebSocketSubscriptionMessage subscriptionMessage)
        {
            var serializeOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedSubscription = JsonSerializer.Serialize(subscriptionMessage, serializeOptions);

            MessageQueue.Enqueue(serializedSubscription);
            MessageAvailable.Set();
        }
    }
}
