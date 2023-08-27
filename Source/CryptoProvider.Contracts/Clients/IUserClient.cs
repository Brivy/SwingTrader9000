using CryptoProvider.Contracts.Models.Api;

namespace CryptoProvider.Contracts.Clients
{
    public interface IUserClient
    {
        Task<IEnumerable<AccountData>> GetAccountsAsync(CancellationToken cancellationToken = default);
    }
}