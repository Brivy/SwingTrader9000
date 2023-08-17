namespace SwingTrader9000.Contracts.Models
{
    public record SymbolTickerSubscriptionMessage
    {
        public string Id { get; init; } = null!;
        public string Type { get; init; } = null!;
        public string Topic { get; init; } = null!;
        public bool PrivateChannel { get; init; }
        public bool Response { get; init; }
    }
}
