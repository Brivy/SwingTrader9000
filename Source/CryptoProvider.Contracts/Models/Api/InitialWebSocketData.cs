namespace CryptoProvider.Contracts.Models.Api
{
    public record InitialWebSocketData
    {
        public string Endpoint { get; init; } = null!;
        public string ConnectId { get; init; } = null!;
        public string Token { get; init; } = null!;
        public int PingInterval { get; init; }
        public int PingTimeout { get; init; }
    }
}
