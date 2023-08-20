using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.Contracts.Models;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Models;

namespace CryptoProvider.KuCoin.Clients
{
    public partial class KuCoinClient
    {
        public async Task<CurrentPrice> GetTickerAsync(string ticker, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string> { { "symbol", ticker } };
            var url = _cryptoProviderUrlService.ConstructUrl(ApiVersion.v1, Endpoints.MarketTicker, queryParams);
            var response = await SendAsync<TickerResponse>(HttpMethod.Get, url, cancellationToken);
            return ConvertToTickerData(response);
        }

        private static CurrentPrice ConvertToTickerData(TickerResponse ticker)
        {
            if (ticker.Data is null) throw new CryptoProviderRequestException("The received response was invalid");
            return new CurrentPrice
            {
                Price = ticker.Data.Price
            };
        }
    }
}
