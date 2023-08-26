namespace CryptoProvider.Contracts.Models
{
    public record AccountData
    {
        public string Id { get; init; } = null!;
        public string Currency { get; init; } = null!;
        public string Type { get; init; } = null!;
        public string Balance { get; init; } = null!;
        public string Available { get; init; } = null!;
        public string Holds { get; init; } = null!;
    }
}
