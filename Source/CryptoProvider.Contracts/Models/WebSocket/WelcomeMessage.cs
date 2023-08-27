using CryptoProvider.Contracts.Interfaces;

namespace CryptoProvider.Contracts.Models.WebSocket
{
    public record WelcomeMessage : IWebSocketMessage
    {
        public string Type { get; init; } = null!;
    }
}
