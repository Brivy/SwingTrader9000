using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.WebSocket;
using CryptoProvider.KuCoin.Clients;
using CryptoProvider.KuCoin.Interfaces;
using CryptoProvider.KuCoin.Queues;
using CryptoProvider.KuCoin.Services;
using CryptoProvider.KuCoin.Settings;
using CryptoProvider.KuCoin.WebSocket;
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

            services.AddHttpClient<IMarketClient, MarketClient>(client => SetupHttpClients(client, settings));
            services.AddHttpClient<IUserClient, UserClient>(client => SetupHttpClients(client, settings));
            services.AddHttpClient<IWebSocketClient, WebSocketClient>(client => SetupHttpClients(client, settings));
            services.AddHttpClient<IOrderClient, OrderClient>(client => SetupHttpClients(client, settings));

            services.AddSingleton<ConcurrentMessageQueue>();

            services
                .AddScoped<ICryptoWebSocket, KuCoinWebSocket>()
                .AddScoped<IPrivateWebSocket, PrivateWebSocket>()
                .AddScoped<IPublicWebSocket, PublicWebSocket>()
                .AddScoped<ICryptoWebSocket, KuCoinWebSocket>()
                .AddScoped<IKuCoinRequestService, KuCoinRequestService>()
                .AddScoped<IKuCoinClientUrlService, KuCoinClientUrlService>();
        }

        private static void SetupHttpClients(HttpClient httpClient, KuCoinSettings settings)
        {
            httpClient.BaseAddress = new Uri(settings.BaseUrl);
        }
    }
}
