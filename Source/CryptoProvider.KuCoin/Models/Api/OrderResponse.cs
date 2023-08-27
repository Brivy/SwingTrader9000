using System.Text.Json.Serialization;

namespace CryptoProvider.KuCoin.Models.Api
{
    public record OrderResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; init; } = null!;
        [JsonPropertyName("msg")]
        public string? Msg { get; init; }
        [JsonPropertyName("data")]
        public OrderResponseData? Data { get; init; } = null!;
    }

    public record OrderResponseData
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; init; } = null!;
    }
}
