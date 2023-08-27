namespace CryptoProvider.Contracts.Models.Api
{
    public record LimitOrderRequest
    {
        public string ClientOid { get; init; } = null!;
        public string Side { get; init; } = null!;
        public string Symbol { get; init; } = null!;
        public string? Type { get; init; }
        public string? Remark { get; init; }
        public string? Stp { get; init; }
        public string? TradeType { get; init; }
        public string Price { get; init; } = null!;
        public string Size { get; init; } = null!;
        public string? TimeInForce { get; init; }
        public long? CancelAfter { get; init; }
        public bool? PostOnly { get; init; }
        public bool? Hidden { get; init; }
        public bool? Iceberg { get; init; }
        public string? VisibleSize { get; init; }
    }
}
