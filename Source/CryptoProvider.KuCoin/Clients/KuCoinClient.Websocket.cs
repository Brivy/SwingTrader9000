using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.Contracts.Models;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Models;
using System.Security.Cryptography;

namespace CryptoProvider.KuCoin.Clients
{
    public partial class KuCoinClient
    {
        public async Task<InitialWebSocketData> GetInitialWebSocketDataAsync(CancellationToken cancellationToken)
        {
            var url = _cryptoProviderUrlService.ConstructUrl(ApiVersion.v1, Endpoints.BulletPublic);
            var response = await PostAsync(url, cancellationToken);
            return ConvertToInitialWebSocketData(response);
        }

        private static InitialWebSocketData ConvertToInitialWebSocketData(BulletPublic bulletPublic)
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
