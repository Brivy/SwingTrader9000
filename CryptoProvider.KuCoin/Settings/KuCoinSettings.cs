using System.ComponentModel.DataAnnotations;

namespace CryptoProvider.KuCoin.Settings
{
    public record KuCoinSettings
    {
        [Required]
        public string BaseUrl { get; init; } = null!;
    }
}
