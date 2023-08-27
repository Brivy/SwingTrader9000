using System.Text.Json.Serialization;

namespace CryptoProvider.KuCoin.Models.Api
{
    public record BulletPublicResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; init; } = null!;
        [JsonPropertyName("msg")]
        public string? Msg { get; init; }
        [JsonPropertyName("data")]
        public BulletPublicResponseData? Data { get; init; } = null!;
    }

    public record BulletPublicResponseData
    {
        [JsonPropertyName("token")]
        public string Token { get; init; } = null!;
        [JsonPropertyName("instanceServers")]
        public IReadOnlyList<BulletPublicResponseInstanceServers> InstanceServers { get; init; } = new List<BulletPublicResponseInstanceServers>();
    }

    public record BulletPublicResponseInstanceServers
    {
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; init; } = null!;
        [JsonPropertyName("encrypt")]
        public bool Encrypt { get; init; }
        [JsonPropertyName("protocol")]
        public string Protocol { get; init; } = null!;
        [JsonPropertyName("pingInterval")]
        public int PingInterval { get; init; }
        [JsonPropertyName("pingTimeout")]
        public int PingTimeout { get; init; }
    }
}
