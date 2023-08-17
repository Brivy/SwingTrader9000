namespace SwingTrader9000.Contracts.Services
{
    public interface IWebSocketService
    {
        Task Initialize(CancellationToken cancellationToken = default);
    }
}