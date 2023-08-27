using CryptoProvider.Contracts.Clients;
using CryptoProvider.Contracts.Exceptions;
using CryptoProvider.Contracts.Models.Api;
using CryptoProvider.KuCoin.Constants;
using CryptoProvider.KuCoin.Enums;
using CryptoProvider.KuCoin.Interfaces;
using CryptoProvider.KuCoin.Models.Api;

namespace CryptoProvider.KuCoin.Clients
{
    public class OrderClient : KuCoinClient, IOrderClient
    {
        private readonly IKuCoinClientUrlService _kuCoinClientUrlService;
        private readonly IKuCoinRequestService _kuCoinRequestService;

        public OrderClient(HttpClient httpClient,
            IKuCoinClientUrlService kuCoinClientUrlService,
            IKuCoinRequestService kuCoinRequestService) : base(httpClient)
        {
            _kuCoinClientUrlService = kuCoinClientUrlService;
            _kuCoinRequestService = kuCoinRequestService;
        }

        public async Task<Order> CreateLimitOrderAsync(LimitOrderRequest limitOrderRequest, CancellationToken cancellationToken = default)
        {
            var url = _kuCoinClientUrlService.ConstructUrl(ApiVersion.v1, Endpoint.Private.Orders);
            var request = _kuCoinRequestService.CreatePrivateRequest(HttpMethod.Post, limitOrderRequest, url);
            var response = await SendAsync<OrderResponse>(request, cancellationToken);
            return ConvertToOrder(response);
        }

        private static Order ConvertToOrder(OrderResponse order)
        {
            if (order.Data is null) throw new CryptoProviderRequestException("The received response was invalid");
            return new Order
            {
                OrderId = order.Data.OrderId
            };
        }
    }
}
