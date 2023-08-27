using CryptoProvider.Contracts.Interfaces;

namespace CryptoProvider.Contracts.Models.WebSocket
{
    public record CurrentPrice : IWebSocketMessage
    {
        public string Price { get; init; } = null!;
    }
}
