namespace CryptoProvider.Contracts.Models
{
    public record CurrentPrice
    {
        public string Price { get; init; } = null!;
    }
}
