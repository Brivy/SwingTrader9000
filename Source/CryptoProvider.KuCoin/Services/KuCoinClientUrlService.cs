using System.Web;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Interfaces;

namespace CryptoProvider.KuCoin.Services
{
    public class KuCoinClientUrlService : IKuCoinClientUrlService
    {
        public string ConstructUrl(ApiVersion apiVersion, string endpoint) =>
            $"api/{apiVersion.ToString().ToLower()}/{endpoint}";

        public string ConstructUrl(ApiVersion apiVersion, string endpoint, Dictionary<string, string> queryParams)
        {
            var url = ConstructUrl(apiVersion, endpoint);
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var (key, value) in queryParams)
            {
                query[key] = value;
            }

            return $"{url}?{query}";
        }
    }
}
