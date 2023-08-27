using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Constants;
using CryptoProvider.Contracts.Models.Api;
using SwingTrader9000.Contracts.Services;

namespace SwingTrader9000.Business.Services
{
    public class TradeService : ITradeService
    {
        private readonly IOrderClient _orderClient;

        public TradeService(IOrderClient orderClient)
        {
            _orderClient = orderClient;
        }

        public async Task CreateLimitOrderAsync()
        {
            var requestBody = new LimitOrderRequest
            {
                ClientOid = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                Side = Side.Sell,
                Symbol = Symbol.XRDUSDT,
                Price = "6.9",
                Size = "10"
            };

            await _orderClient.CreateLimitOrderAsync(requestBody);
        }
    }
}
