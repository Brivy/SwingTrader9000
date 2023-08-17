using System.Collections.Concurrent;

namespace SwingTrader9000.Business.Queues
{
    public class ConcurrentMessageQueue
    {
        public ConcurrentQueue<string> MessageQueue { get; } = new ConcurrentQueue<string>();
        public AutoResetEvent MessageAvailable { get; } = new AutoResetEvent(false);
    }
}
