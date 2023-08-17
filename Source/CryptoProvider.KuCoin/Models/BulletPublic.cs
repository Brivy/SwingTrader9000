using System.Text.Json.Serialization;

namespace CryptoProvider.KuCoin.Models
{
    public record BulletPublic
    {
        [JsonPropertyName("code")]
        public string Code { get; init; } = null!;
        [JsonPropertyName("data")]
        public BulletPublicData Data { get; init; } = null!;
    }

    public record BulletPublicData
    {
        [JsonPropertyName("token")]
        public string Token { get; init; } = null!;
        [JsonPropertyName("instanceServers")]
        public IReadOnlyList<BulletPublicInstanceServers> InstanceServers { get; init; } = new List<BulletPublicInstanceServers>();
    }

    public record BulletPublicInstanceServers
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
