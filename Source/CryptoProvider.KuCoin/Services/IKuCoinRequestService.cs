namespace CryptoProvider.KuCoin.Services
{
    public interface IKuCoinRequestService
    {
        HttpRequestMessage CreatePrivateRequest(HttpMethod httpMethod, string url);
        HttpRequestMessage CreatePublicRequest(HttpMethod httpMethod, string url);
    }
}