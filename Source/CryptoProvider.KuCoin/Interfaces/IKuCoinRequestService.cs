namespace CryptoProvider.KuCoin.Interfaces
{
    public interface IKuCoinRequestService
    {
        HttpRequestMessage CreatePrivateRequest(HttpMethod httpMethod, string url);
        HttpRequestMessage CreatePublicRequest(HttpMethod httpMethod, string url);
    }
}