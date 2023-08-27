using CryptoProvider.Contracts.Interfaces;

namespace CryptoProvider.Contracts.Models.WebSocket
{
    public record BasicMessage : IWebSocketMessage
    {
        public string Type { get; init; } = null!;
    }
}
