namespace SwingTrader9000.Contracts.Services
{
    public interface IWebSocketService
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
