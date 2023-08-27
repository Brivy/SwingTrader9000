using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Web;
using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Interfaces;
using CryptoProvider.Contracts.Models.WebSocket;
using CryptoProvider.Contracts.WebSocket;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Models.WebSocket;
using CryptoProvider.KuCoin.Queues;
using Microsoft.Extensions.Logging;

namespace CryptoProvider.KuCoin.WebSocket
{
    public class KuCoinWebSocket : ICryptoWebSocket
    {
        private readonly IWebSocketClient _webSocketClient;
        private readonly ConcurrentMessageQueue _concurrentMessageQueue;
        private readonly ILogger<KuCoinWebSocket> _logger;

        public KuCoinWebSocket(
            IWebSocketClient webSocketClient,
            ConcurrentMessageQueue concurrentMessageQueue,
            ILogger<KuCoinWebSocket> logger)
        {
            _webSocketClient = webSocketClient;
            _concurrentMessageQueue = concurrentMessageQueue;
            _logger = logger;
        }

        public async Task InitializeAsync(Func<IWebSocketMessage, bool> callback, Func<IWebSocketMessage, CancellationToken, Task<bool>> asyncCallback, CancellationToken cancellationToken = default)
        {
            var webSocketData = await _webSocketClient.GetPrivateWebSocketDataAsync(cancellationToken);
            var webSocketUri = CreateWebSocketUri(webSocketData.Endpoint, webSocketData.Token, webSocketData.ConnectId);

            using var client = new ClientWebSocket();
            await client.ConnectAsync(webSocketUri, cancellationToken);
            await Task.WhenAll(ReceiveAsync(client, callback, asyncCallback, cancellationToken), SendAsync(client, cancellationToken));
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
                case var m when m.Contains(Topic.TradeOrders):
                    var tradeOrdersMessage = JsonSerializer.Deserialize<TradeOrderMessage>(message);
                    if (tradeOrdersMessage == null) return null;
                    return new TradeOrder
                    {
                        CanceledFunds = tradeOrdersMessage.Data.CanceledFunds,
                        CanceledSize = tradeOrdersMessage.Data.CanceledSize,
                        ClientOid = tradeOrdersMessage.Data.ClientOid,
                        FilledSize = tradeOrdersMessage.Data.FilledSize,
                        Liquidity = tradeOrdersMessage.Data.Liquidity,
                        MatchPrice = tradeOrdersMessage.Data.MatchPrice,
                        MatchSize = tradeOrdersMessage.Data.MatchSize,
                        OrderId = tradeOrdersMessage.Data.OrderId,
                        OrderTime = tradeOrdersMessage.Data.OrderTime,
                        OrderType = tradeOrdersMessage.Data.OrderType,
                        OriginFunds = tradeOrdersMessage.Data.OriginFunds,
                        OriginSize = tradeOrdersMessage.Data.OriginSize,
                        Price = tradeOrdersMessage.Data.Price,
                        RemainSize = tradeOrdersMessage.Data.RemainSize,
                        Side = tradeOrdersMessage.Data.Side,
                        Size = tradeOrdersMessage.Data.Size,
                        Status = tradeOrdersMessage.Data.Status,
                        Symbol = tradeOrdersMessage.Data.Symbol,
                        TradeId = tradeOrdersMessage.Data.TradeId,
                        Ts = tradeOrdersMessage.Data.Ts,
                        Type = tradeOrdersMessage.Data.Type
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
