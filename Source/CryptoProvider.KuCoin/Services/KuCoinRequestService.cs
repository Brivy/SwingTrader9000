using System.Security.Cryptography;
using System.Text;
using CryptoProvider.KuCoin.Settings;
using Microsoft.Extensions.Options;

namespace CryptoProvider.KuCoin.Services
{
    public class KuCoinRequestService : IKuCoinRequestService
    {
        private readonly KuCoinSettings _options;

        public KuCoinRequestService(IOptions<KuCoinSettings> options)
        {
            _options = options.Value;
        }

        public HttpRequestMessage CreatePublicRequest(HttpMethod httpMethod, string url)
        {
            return new HttpRequestMessage(httpMethod, url);
        }

        public HttpRequestMessage CreatePrivateRequest(HttpMethod httpMethod, string url)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            var requestPath = request.RequestUri?.OriginalString;
            var method = httpMethod.Method.ToUpper();
            var timestamp = GetCurrentUnixTimestampMillis();
            var preHashString = $"{timestamp}{method}/{requestPath}";
            var signature = ComputeSignature(preHashString, _options.ApiSecret);
            var passphrase = ComputeSignature(_options.Passphrase, _options.ApiSecret);

            request.Headers.Add("KC-API-KEY", _options.ApiKey);
            request.Headers.Add("KC-API-SIGN", signature);
            request.Headers.Add("KC-API-PASSPHRASE", passphrase);
            request.Headers.Add("KC-API-KEY-VERSION", _options.ApiVersion);
            request.Headers.Add("KC-API-TIMESTAMP", timestamp);

            return request;
        }

        private static string GetCurrentUnixTimestampMillis()
        {
            var epochStart = DateTime.UnixEpoch;
            var totalMilliseconds = (long)(DateTime.UtcNow - epochStart).TotalMilliseconds;
            return totalMilliseconds.ToString();
        }

        private static string ComputeSignature(string message, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            using var hasher = new HMACSHA256(keyBytes);
            var hashBytes = hasher.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
