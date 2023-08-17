using System.Text.Json.Serialization;

namespace SwingTrader9000.Contracts.Models
{
    public record SymbolTickerMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = null!;
        [JsonPropertyName("topic")]
        public string Topic { get; init; } = null!;
        [JsonPropertyName("subject")]
        public string Subject { get; init; } = null!;
        [JsonPropertyName("data")]
        public SymbolTickerMessageData Data { get; init; } = null!;
    }

    public record SymbolTickerMessageData
    {
        [JsonPropertyName("sequence")]
        public string Sequence { get; init; } = null!;
        [JsonPropertyName("price")]
        public string Price { get; init; } = null!;
        [JsonPropertyName("size")]
        public string Size { get; init; } = null!;
        [JsonPropertyName("bestAsk")]
        public string BestAsk { get; init; } = null!;
        [JsonPropertyName("bestAskSize")]
        public string BestAskSize { get; init; } = null!;
        [JsonPropertyName("bestBid")]
        public string BestBid { get; init; } = null!;
        [JsonPropertyName("bestBidSize")]
        public string BestBidSize { get; init; } = null!;
    }
}
