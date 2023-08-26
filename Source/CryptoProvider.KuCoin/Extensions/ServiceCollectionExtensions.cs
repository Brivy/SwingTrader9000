using CryptoProvider.Contracts.Clients;
using CryptoProvider.KuCoin.Clients;
using CryptoProvider.KuCoin.Services;
using CryptoProvider.KuCoin.Settings;
using DependencyInjection.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoProvider.KuCoin.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureCryptoProviderServices(this IServiceCollection services, IConfiguration configuration)
        {
            var kuCoinSettings = configuration.GetSection(nameof(KuCoinSettings));
            services.AddOptionsWithValidation<KuCoinSettings>(kuCoinSettings);

            var settings = kuCoinSettings.Get<KuCoinSettings>() ?? throw new NullReferenceException();
            services.AddHttpClient<ICryptoClient, KuCoinClient>(client =>
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

            services
                .AddScoped<IKuCoinRequestService, KuCoinRequestService>()
                .AddScoped<IKuCoinClientUrlService, KuCoinClientUrlService>();
        }
    }
}
