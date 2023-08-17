namespace SwingTrader9000.Business.Services
{
    public interface IProcessMessageService
    {
        void ProcessWelcomeMessage();
        void ProcessSymbolTickerMessage(string message);
    }
}