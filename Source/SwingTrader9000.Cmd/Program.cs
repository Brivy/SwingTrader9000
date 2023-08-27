using CryptoProvider.KuCoin.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SwingTrader9000.Business.Extensions;
using SwingTrader9000.Contracts.Services;

namespace SwingTrader9000.Cmd
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
               .Build();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigureCryptoProviderServices(configuration);
                    services.ConfigureSwingTrader9000Services(configuration);
                })
                .Build();

            using var serviceScope = host.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            //var webSocketService = serviceProvider.GetRequiredService<IWebSocketService>();
            var tradeService = serviceProvider.GetRequiredService<ITradeService>();
            await tradeService.CreateLimitOrderAsync();
            //await webSocketService.InitializeAsync(CancellationToken.None);
        }
    }
}
