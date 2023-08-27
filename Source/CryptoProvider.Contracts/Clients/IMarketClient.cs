using CryptoProvider.Contracts.Models.Api;

namespace CryptoProvider.Contracts.Clients
{
    public interface IMarketClient
    {
        Task<CurrentPrice> GetTickerAsync(string ticker, CancellationToken cancellationToken = default);
    }
}