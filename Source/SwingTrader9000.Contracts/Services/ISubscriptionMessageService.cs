using SwingTrader9000.Contracts.Models;

namespace SwingTrader9000.Contracts.Services
{
    public interface ISubscriptionMessageService
    {
        SymbolTickerSubscriptionMessage CreateSymbolTickerSubscriptionMessage(List<string> symbols);
    }
}