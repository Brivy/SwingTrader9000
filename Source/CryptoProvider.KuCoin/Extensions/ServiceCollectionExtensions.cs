using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Services;
using CryptoProvider.KuCoin.Clients;
using CryptoProvider.KuCoin.Interfaces;
using CryptoProvider.KuCoin.Queues;
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

            services.AddSingleton<ConcurrentMessageQueue>();
            services
                .AddScoped<IKuCoinWebSocketService, KuCoinWebSocketService>()
                .AddScoped<IKuCoinRequestService, KuCoinRequestService>()
                .AddScoped<IKuCoinClientUrlService, KuCoinClientUrlService>();
        }
    }
}
