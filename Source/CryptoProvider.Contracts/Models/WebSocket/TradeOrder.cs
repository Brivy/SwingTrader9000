using CryptoProvider.Contracts.Interfaces;

namespace CryptoProvider.Contracts.Models.WebSocket
{
    public record TradeOrder : IWebSocketMessage
    {
        public string Symbol { get; init; } = null!;
        public string OrderType { get; init; } = null!;
        public string Side { get; init; } = null!;
        public string OrderId { get; init; } = null!;
        public string? Liquidity { get; init; }
        public string Type { get; init; } = null!;
        public long OrderTime { get; init; }
        public string? Size { get; init; }
        public string? FilledSize { get; init; }
        public string Price { get; init; } = null!;
        public string? MatchPrice { get; init; }
        public string? MatchSize { get; init; }
        public string? TradeId { get; init; }
        public string ClientOid { get; init; } = null!;
        public string? RemainSize { get; init; }
        public string Status { get; init; } = null!;
        public string? CanceledSize { get; init; }
        public string? CanceledFunds { get; init; }
        public string OriginSize { get; init; } = null!;
        public string OriginFunds { get; init; } = null!;
        public long Ts { get; init; }
    }
}
