using System.Text.Json.Serialization;

namespace CryptoProvider.KuCoin.Models
{
    public record TickerResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; init; } = null!;
        [JsonPropertyName("data")]
        public TickerResponseData Data { get; init; } = null!;
    }

    public record TickerResponseData
    {
        [JsonPropertyName("sequence")]
        public string Sequence { get; init; } = null!;
        [JsonPropertyName("bestAsk")]
        public string BestAsk { get; init; } = null!;
        [JsonPropertyName("size")]
        public string Size { get; init; } = null!;
        [JsonPropertyName("price")]
        public string Price { get; init; } = null!;
        [JsonPropertyName("bestBidSize")]
        public string BestBidSize { get; init; } = null!;
        [JsonPropertyName("bestBid")]
        public string BestBid { get; init; } = null!;
        [JsonPropertyName("bestAskSize")]
        public string BestAskSize { get; init; } = null!;
        [JsonPropertyName("time")]
        public long Time { get; init; }
    }
}
