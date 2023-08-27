namespace CryptoProvider.KuCoin.Interfaces
{
    public interface IKuCoinRequestService
    {
        HttpRequestMessage CreatePrivateRequest(HttpMethod httpMethod, string url);
        HttpRequestMessage CreatePrivateRequest<TRequestBody>(HttpMethod httpMethod, TRequestBody requestBody, string url);
        HttpRequestMessage CreatePublicRequest(HttpMethod httpMethod, string url);
    }
}
