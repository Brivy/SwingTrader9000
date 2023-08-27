namespace CryptoProvider.Contracts.Models.Api
{
    public record CurrentPrice
    {
        public string Price { get; init; } = null!;
    }
}
