using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.Contracts.Models;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Models;

namespace CryptoProvider.KuCoin.Clients
{
    public partial class KuCoinClient
    {
        public async Task<IEnumerable<AccountData>> GetAccountsAsync(CancellationToken cancellationToken = default)
        {
            var url = _kuCoinUrlService.ConstructUrl(ApiVersion.v1, PrivateEndpoint.Accounts);
            var request = _kuCoinRequestService.CreatePrivateRequest(HttpMethod.Get, url);
            var response = await SendPublicAsync<AccountsResponse>(request, cancellationToken);
            return ConvertToTickerData(response);
        }

        private static IEnumerable<AccountData> ConvertToTickerData(AccountsResponse accounts)
        {
            if (accounts.Data is null) throw new CryptoProviderRequestException("The received response was invalid");
            return accounts.Data.Select(x => new AccountData
            {
                Id = x.Id,
                Balance = x.Balance,
                Available = x.Available,
                Holds = x.Holds,
                Currency = x.Currency
            }).ToList();
        }
    }
}
