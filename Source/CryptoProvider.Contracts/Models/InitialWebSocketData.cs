namespace CryptoProvider.Contracts.Models
{
    public record InitialWebSocketData
    {
        public string? Endpoint { get; init; }
        public string? Token { get; init; }
        public int? PingInterval { get; init; }
        public int? PingTimeout { get; init; }
    }
}
