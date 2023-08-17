using System.Runtime.Serialization;

namespace CryptoProvider.KuCoin.Exceptions
{
    [Serializable]
    public class KuCoinInvalidResponseException : Exception
    {
        public KuCoinInvalidResponseException()
        {
        }

        public KuCoinInvalidResponseException(string message) : base(message)
        {
        }

        public KuCoinInvalidResponseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected KuCoinInvalidResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
