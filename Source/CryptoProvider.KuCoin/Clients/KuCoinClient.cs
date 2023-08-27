using System.Net.Http.Json;
using System.Text.Json;
using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.KuCoin.Exceptions;
using CryptoProvider.KuCoin.Interfaces;

namespace CryptoProvider.KuCoin.Clients
{
    public partial class KuCoinClient : ICryptoClient
    {
        private readonly HttpClient _httpClient;
        private readonly IKuCoinClientUrlService _kuCoinUrlService;
        private readonly IKuCoinRequestService _kuCoinRequestService;

        public KuCoinClient(
            HttpClient httpClient,
            IKuCoinClientUrlService kuCoinUrlService,
            IKuCoinRequestService kuCoinRequestService)
        {
            _httpClient = httpClient;
            _kuCoinUrlService = kuCoinUrlService;
            _kuCoinRequestService = kuCoinRequestService;
        }

        private async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.SendAsync(request, cancellationToken);
                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw new KuCoinInvalidResponseException("The HTTP content is empty or null");
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
