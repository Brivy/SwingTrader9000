namespace CryptoProvider.Contracts.WebSocket
{
    public interface IPublicWebSocket
    {
        void SubscribeToSymbolTicker(List<string> symbols);
    }
}