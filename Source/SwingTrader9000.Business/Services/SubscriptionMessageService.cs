using SwingTrader9000.Business.Constants;
using SwingTrader9000.Contracts.Models;
using SwingTrader9000.Contracts.Services;

namespace SwingTrader9000.Business.Services
{
    public class SubscriptionMessageService : ISubscriptionMessageService
    {
        public SymbolTickerSubscriptionMessage CreateSymbolTickerSubscriptionMessage(List<string> symbols)
        {
            if (symbols is null || !symbols.Any()) throw new ArgumentException("No symbols provided");

            return new SymbolTickerSubscriptionMessage
            {
                Id = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                Type = MessageType.Subscribe,
                Topic = $"{Topic.Ticker}:{string.Join(",", symbols)}",
                PrivateChannel = false,
                Response = true
            };
        }
    }
}
