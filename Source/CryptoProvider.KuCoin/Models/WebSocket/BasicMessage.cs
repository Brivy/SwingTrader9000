using System.Text.Json.Serialization;

namespace CryptoProvider.KuCoin.Models.WebSocket
{
    public record BasicMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = null!;
        [JsonPropertyName("type")]
        public string Type { get; init; } = null!;
    }
}
