using CryptoProvider.KuCoin.Enums;

namespace CryptoProvider.KuCoin.Services
{
    public interface IKuCoinClientUrlService
    {
        string ConstructUrl(ApiVersion apiVersion, string endpoint);
    }
}