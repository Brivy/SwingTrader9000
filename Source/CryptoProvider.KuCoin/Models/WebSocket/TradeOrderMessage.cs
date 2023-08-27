using System.Text.Json.Serialization;

namespace CryptoProvider.KuCoin.Models.WebSocket
{
    public record TradeOrderMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = null!;
        [JsonPropertyName("topic")]
        public string Topic { get; init; } = null!;
        [JsonPropertyName("subject")]
        public string Subject { get; init; } = null!;
        [JsonPropertyName("channelType")]
        public string ChannelType { get; init; } = null!;
        [JsonPropertyName("data")]
        public TradeOrderMessageData Data { get; init; } = null!;
    }

    public record TradeOrderMessageData
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; init; } = null!;
        [JsonPropertyName("orderType")]
        public string OrderType { get; init; } = null!;
        [JsonPropertyName("side")]
        public string Side { get; init; } = null!;
        [JsonPropertyName("orderId")]
        public string OrderId { get; init; } = null!;
        [JsonPropertyName("liquidity")]
        public string? Liquidity { get; init; }
        [JsonPropertyName("type")]
        public string Type { get; init; } = null!;
        [JsonPropertyName("orderTime")]
        public long OrderTime { get; init; }
        [JsonPropertyName("size")]
        public string? Size { get; init; }
        [JsonPropertyName("filledSize")]
        public string? FilledSize { get; init; }
        [JsonPropertyName("price")]
        public string Price { get; init; } = null!;
        [JsonPropertyName("matchPrice")]
        public string? MatchPrice { get; init; }
        [JsonPropertyName("matchSize")]
        public string? MatchSize { get; init; }
        [JsonPropertyName("tradeId")]
        public string? TradeId { get; init; }
        [JsonPropertyName("clientOid")]
        public string ClientOid { get; init; } = null!;
        [JsonPropertyName("remainSize")]
        public string? RemainSize { get; init; }
        [JsonPropertyName("status")]
        public string Status { get; init; } = null!;
        [JsonPropertyName("canceledSize")]
        public string? CanceledSize { get; init; }
        [JsonPropertyName("canceledFunds")]
        public string? CanceledFunds { get; init; }
        [JsonPropertyName("originSize")]
        public string OriginSize { get; init; } = null!;
        [JsonPropertyName("originFunds")]
        public string OriginFunds { get; init; } = null!;
        [JsonPropertyName("ts")]
        public long Ts { get; init; }
    }
}
