using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.Contracts.Models.Api;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Models.Api;

namespace CryptoProvider.KuCoin.Clients
{
    public partial class KuCoinClient
    {
        public async Task<CurrentPrice> GetTickerAsync(string ticker, CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string> { { "symbol", ticker } };
            var url = _kuCoinUrlService.ConstructUrl(ApiVersion.v1, PublicEndpoint.MarketTicker, queryParams);
            var request = _kuCoinRequestService.CreatePublicRequest(HttpMethod.Get, url);
            var response = await SendAsync<TickerResponse>(request, cancellationToken);
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
