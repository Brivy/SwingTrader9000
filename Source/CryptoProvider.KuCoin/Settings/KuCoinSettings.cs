using System.ComponentModel.DataAnnotations;

namespace CryptoProvider.KuCoin.Settings
{
    public record KuCoinSettings
    {
        [Required]
        public string BaseUrl { get; init; } = null!;

        [Required]
        public string ApiKey { get; init; } = null!;

        [Required]
        public string ApiSecret { get; init; } = null!;

        [Required]
        public string Passphrase { get; init; } = null!;

        [Required]
        public string ApiVersion { get; init; } = null!;
    }
}
