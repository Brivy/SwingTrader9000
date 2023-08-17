using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.KuCoin.Exceptions;
using CryptoProvider.KuCoin.Models;
using CryptoProvider.KuCoin.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace CryptoProvider.KuCoin.Clients
{
    public partial class KuCoinClient : ICryptoClient
    {
        private readonly HttpClient _httpClient;
        private readonly IKuCoinClientUrlService _cryptoProviderUrlService;

        public KuCoinClient(
            HttpClient httpClient,
            IKuCoinClientUrlService cryptoProviderUrlService)
        {
            _httpClient = httpClient;
            _cryptoProviderUrlService = cryptoProviderUrlService;
        }

        public async Task<BulletPublic> PostAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var response = await _httpClient.SendAsync(request, cancellationToken);
                return await response.Content.ReadFromJsonAsync<BulletPublic>(cancellationToken: cancellationToken) ?? throw new KuCoinInvalidResponseException("The HTTP content is empty or null");
            }
            catch (KuCoinInvalidResponseException ex)
            {
                throw new CryptoProviderRequestException($"Could not deserialize response body: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new CryptoProviderRequestException($"Could not deserialize response body: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new CryptoProviderRequestException($"The request ended in an exception: {ex.Message}");
            }
        }
    }
}
