using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwingTrader9000.Business.Services;
using SwingTrader9000.Contracts.Services;

namespace SwingTrader9000.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureSwingTrader9000Services(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IProcessWebSocketMessageService, ProcessWebSocketMessageService>()
                .AddScoped<IWebSocketService, WebSocketService>();
        }
    }
}
