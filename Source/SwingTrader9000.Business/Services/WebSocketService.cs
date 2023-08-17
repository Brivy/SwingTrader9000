using CryptoProvider.Contracts.Clients;
using Microsoft.Extensions.Logging;
using SwingTrader9000.Business.Constants;
using SwingTrader9000.Business.Queues;
using SwingTrader9000.Contracts.Services;
using System.Net.WebSockets;
using System.Text;
using System.Web;

namespace SwingTrader9000.Business.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly ICryptoClient _cryptoClient;
        private readonly IProcessMessageService _processMessageService;
        private readonly ConcurrentMessageQueue _concurrentMessageQueue;
        private readonly ILogger<WebSocketService> _logger;

        public WebSocketService(
            ICryptoClient cryptoClient,
            IProcessMessageService processMessageService,
            ConcurrentMessageQueue concurrentMessageQueue,
            ILogger<WebSocketService> logger)
        {
            _cryptoClient = cryptoClient;
            _processMessageService = processMessageService;
            _concurrentMessageQueue = concurrentMessageQueue;
            _logger = logger;
        }

        public async Task Initialize(CancellationToken cancellationToken = default)
        {
            var webSocketData = await _cryptoClient.GetInitialWebSocketDataAsync(cancellationToken);
            var webSocketUri = CreateWebSocketUri(webSocketData.Endpoint, webSocketData.Token, webSocketData.ConnectId);

            using var client = new ClientWebSocket();
            await client.ConnectAsync(webSocketUri, cancellationToken);
            await Task.WhenAll(ReceiveAsync(client), SendAsync(client));
        }

        private async Task SendAsync(ClientWebSocket client)
        {
            while (client.State == WebSocketState.Open)
            {
                _concurrentMessageQueue.MessageAvailable.WaitOne();
                if (_concurrentMessageQueue.MessageQueue.TryDequeue(out var message))
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private async Task ReceiveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];

            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessMessageType(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogWarning("Connection closed.");
                    break;
                }
            }
        }

        private void ProcessMessageType(string message)
        {
            switch (message)
            {
                case var m when
                    m.Contains(MessageType.Ping) ||
                    m.Contains(MessageType.Pong) ||
                    m.Contains(MessageType.Ack) ||
                    m.Contains(MessageType.Subscribe) ||
                    m.Contains(MessageType.Unsubscribe):
                    _logger.LogInformation("{message}", m);
                    break;
                case var m when m.Contains(MessageType.Welcome):
                    _processMessageService.ProcessWelcomeMessage();
                    break;
                case var m when m.Contains(MessageType.Message):
                    ProcessMessageTopic(m);
                    break;
                default:
                    _logger.LogWarning("Unknown message received: {message}", message);
                    break;
            }
        }

        private void ProcessMessageTopic(string message)
        {
            switch (message)
            {
                case var m when m.Contains(Topic.Ticker):
                    _processMessageService.ProcessSymbolTickerMessage(m);
                    break;
                case var m when m.Contains(Topic.Snapshot):
                    _logger.LogInformation("Balance received.");
                    break;
                default:
                    _logger.LogWarning("Unknown topic received: {message}", message);
                    break;
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
