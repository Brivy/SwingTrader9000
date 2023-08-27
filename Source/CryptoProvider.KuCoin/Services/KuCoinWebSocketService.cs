using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Web;
using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Constants;
using CryptoProvider.Contracts.Interfaces;
using CryptoProvider.Contracts.Models.WebSocket;
using CryptoProvider.Contracts.Services;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Models.WebSocket;
using CryptoProvider.KuCoin.Queues;
using Microsoft.Extensions.Logging;

namespace CryptoProvider.KuCoin.Services
{
    public class KuCoinWebSocketService : IKuCoinWebSocketService
    {
        private readonly ICryptoClient _cryptoClient;
        private readonly ConcurrentMessageQueue _concurrentMessageQueue;
        private readonly ILogger<KuCoinWebSocketService> _logger;

        public KuCoinWebSocketService(
            ICryptoClient cryptoClient,
            ConcurrentMessageQueue concurrentMessageQueue,
            ILogger<KuCoinWebSocketService> logger)
        {
            _cryptoClient = cryptoClient;
            _concurrentMessageQueue = concurrentMessageQueue;
            _logger = logger;
        }

        public async Task InitializeAsync(Func<IWebSocketMessage, bool> callback, Func<IWebSocketMessage, CancellationToken, Task<bool>> asyncCallback, CancellationToken cancellationToken = default)
        {
            var webSocketData = await _cryptoClient.GetPublicWebSocketDataAsync(cancellationToken);
            var webSocketUri = CreateWebSocketUri(webSocketData.Endpoint, webSocketData.Token, webSocketData.ConnectId);

            using var client = new ClientWebSocket();
            await client.ConnectAsync(webSocketUri, cancellationToken);
            await Task.WhenAll(ReceiveAsync(client, callback, asyncCallback, cancellationToken), SendAsync(client, cancellationToken));
        }

        public void EnqueueWebSocketMessage(object message)
        {
            var serializeOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedSubscription = JsonSerializer.Serialize(message, serializeOptions);

            _concurrentMessageQueue.MessageQueue.Enqueue(serializedSubscription);
            _concurrentMessageQueue.MessageAvailable.Set();
        }

        private async Task SendAsync(ClientWebSocket client, CancellationToken cancellationToken = default)
        {
            while (client.State == WebSocketState.Open)
            {
                _concurrentMessageQueue.MessageAvailable.WaitOne();
                if (_concurrentMessageQueue.MessageQueue.TryDequeue(out var message))
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
                }
            }
        }

        private async Task ReceiveAsync(ClientWebSocket client, Func<IWebSocketMessage, bool> callback, Func<IWebSocketMessage, CancellationToken, Task<bool>> asyncCallback, CancellationToken cancellationToken = default)
        {
            var buffer = new byte[1024 * 4];

            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var deserializedMessage = DeserializeMessage(message);
                        if (deserializedMessage == null)
                        {
                            _logger.LogWarning("Failed to deserialize message: {message}", message);
                            break;
                        }

                        var completed = callback(deserializedMessage);
                        if (!completed)
                        {
                            completed = await asyncCallback(deserializedMessage, cancellationToken);
                        }

                        if (!completed)
                        {
                            _logger.LogWarning("Failed to process message: {message}", message);
                        }

                        break;
                    case WebSocketMessageType.Close:
                        _logger.LogWarning("Connection closed.");
                        break;
                    default:
                        _logger.LogWarning("Unknown WebSocket message type received");
                        break;
                }
            }
        }

        private IWebSocketMessage? DeserializeMessage(string message)
        {
            switch (message)
            {
                case var m when
                    m.Contains(ResponseMessageType.Ack) ||
                    m.Contains(ResponseMessageType.Pong) ||
                    m.Contains(ResponseMessageType.Error):
                    var basicMessage = JsonSerializer.Deserialize<Models.WebSocket.BasicMessage>(message);
                    if (basicMessage == null) return null;
                    return new Contracts.Models.WebSocket.BasicMessage
                    {
                        Type = basicMessage.Type
                    };
                case var m when m.Contains(ResponseMessageType.Welcome):
                    var welcomeMessage = JsonSerializer.Deserialize<Models.WebSocket.BasicMessage>(message);
                    if (welcomeMessage == null) return null;
                    return new WelcomeMessage
                    {
                        Type = welcomeMessage.Type
                    };
                case var m when m.Contains(ResponseMessageType.Message):
                    return DeserializeMessageTopic(m);
                default:
                    _logger.LogWarning("Unknown message received: {message}", message);
                    return null;
            }
        }

        private IWebSocketMessage? DeserializeMessageTopic(string message)
        {
            switch (message)
            {
                case var m when m.Contains(Topic.Ticker):
                    var symbolTickerMessage = JsonSerializer.Deserialize<SymbolTickerMessage>(message);
                    if (symbolTickerMessage == null) return null;
                    return new CurrentPrice
                    {
                        Price = symbolTickerMessage.Data.Price
                    };
                default:
                    _logger.LogWarning("Unknown topic received: {message}", message);
                    return null;
            }
        }

        private static Uri CreateWebSocketUri(string endpoint, string token, string connectId)
        {
            var uri = new Uri(endpoint);

            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["connectId"] = connectId;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}
