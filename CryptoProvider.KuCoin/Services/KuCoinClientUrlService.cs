using CryptoProvider.KuCoin.Enums;

namespace CryptoProvider.KuCoin.Services
{
    public class KuCoinClientUrlService : IKuCoinClientUrlService
    {
        public string ConstructUrl(ApiVersion apiVersion, string endpoint) =>
            $"api/v1/{endpoint}";
    }
}
