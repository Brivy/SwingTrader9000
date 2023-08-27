using System.Security.Cryptography;
using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.Contracts.Models.Api;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Interfaces;
using CryptoProvider.KuCoin.Models.Api;

namespace CryptoProvider.KuCoin.Clients
{
    public class WebSocketClient : KuCoinClient, IWebSocketClient
    {
        private readonly IKuCoinClientUrlService _kuCoinClientUrlService;
        private readonly IKuCoinRequestService _kuCoinRequestService;

        public WebSocketClient(HttpClient httpClient,
            IKuCoinClientUrlService kuCoinClientUrlService,
            IKuCoinRequestService kuCoinRequestService) : base(httpClient)
        {
            _kuCoinClientUrlService = kuCoinClientUrlService;
            _kuCoinRequestService = kuCoinRequestService;
        }

        public async Task<InitialWebSocketData> GetPrivateWebSocketDataAsync(CancellationToken cancellationToken = default)
        {
            var url = _kuCoinClientUrlService.ConstructUrl(ApiVersion.v1, Endpoint.Private.BulletPrivate);
            var request = _kuCoinRequestService.CreatePrivateRequest(HttpMethod.Post, url);
            var response = await SendAsync<BulletPublicResponse>(request, cancellationToken);
            return ConvertToInitialWebSocketData(response);
        }

        private static InitialWebSocketData ConvertToInitialWebSocketData(BulletPublicResponse bulletPublic)
        {
            if (bulletPublic.Data is null || bulletPublic.Data.InstanceServers?.Any() is not true) throw new CryptoProviderRequestException("The received response was invalid");
            var instanceServer = bulletPublic.Data.InstanceServers[0];

            return new InitialWebSocketData
            {
                Token = bulletPublic.Data.Token,
                Endpoint = instanceServer.Endpoint,
                ConnectId = CreateConnectId(),
                PingInterval = instanceServer.PingInterval,
                PingTimeout = instanceServer.PingTimeout
            };
        }

        private static string CreateConnectId()
        {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timeBytes = BitConverter.GetBytes(currentTime);
            var hashBytes = MD5.HashData(timeBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
