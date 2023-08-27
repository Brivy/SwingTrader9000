using System.Text.Json.Serialization;

namespace CryptoProvider.KuCoin.Models.Api
{
    public record AccountsResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; init; } = null!;
        [JsonPropertyName("data")]
        public IReadOnlyList<AccountsResponseData> Data { get; init; } = new List<AccountsResponseData>();
    }

    public record AccountsResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = null!;
        [JsonPropertyName("currency")]
        public string Currency { get; init; } = null!;
        [JsonPropertyName("type")]
        public string Type { get; init; } = null!;
        [JsonPropertyName("balance")]
        public string Balance { get; init; } = null!;
        [JsonPropertyName("available")]
        public string Available { get; init; } = null!;
        [JsonPropertyName("holds")]
        public string Holds { get; init; } = null!;
    }
}
