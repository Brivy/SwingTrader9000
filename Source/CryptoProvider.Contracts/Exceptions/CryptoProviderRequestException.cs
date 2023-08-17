using System.Runtime.Serialization;

namespace CryptoProvider.Contracts.Exceptions
{
    [Serializable]
    public class CryptoProviderRequestException : Exception
    {
        public CryptoProviderRequestException()
        {
        }

        public CryptoProviderRequestException(string message) : base(message)
        {
        }

        public CryptoProviderRequestException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CryptoProviderRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
