using System.Collections.Concurrent;

namespace CryptoProvider.KuCoin.Queues
{
    public class ConcurrentMessageQueue
    {
        public ConcurrentQueue<string> MessageQueue { get; } = new ConcurrentQueue<string>();
        public AutoResetEvent MessageAvailable { get; } = new AutoResetEvent(false);
    }
}
