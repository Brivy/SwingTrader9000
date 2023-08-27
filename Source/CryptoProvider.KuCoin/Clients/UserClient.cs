using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.Contracts.Models.Api;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Interfaces;
using CryptoProvider.KuCoin.Models.Api;

namespace CryptoProvider.KuCoin.Clients
{
    public class UserClient : KuCoinClient, IUserClient
    {
        private readonly IKuCoinClientUrlService _kuCoinClientUrlService;
        private readonly IKuCoinRequestService _kuCoinRequestService;

        public UserClient(HttpClient httpClient,
            IKuCoinClientUrlService kuCoinClientUrlService,
            IKuCoinRequestService kuCoinRequestService) : base(httpClient)
        {
            _kuCoinClientUrlService = kuCoinClientUrlService;
            _kuCoinRequestService = kuCoinRequestService;
        }

        public async Task<IEnumerable<AccountData>> GetAccountsAsync(CancellationToken cancellationToken = default)
        {
            var url = _kuCoinClientUrlService.ConstructUrl(ApiVersion.v1, Endpoint.Private.Accounts);
            var request = _kuCoinRequestService.CreatePrivateRequest(HttpMethod.Get, url);
            var response = await SendAsync<AccountsResponse>(request, cancellationToken);
            return ConvertToAccountData(response);
        }

        private static IEnumerable<AccountData> ConvertToAccountData(AccountsResponse accounts)
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
