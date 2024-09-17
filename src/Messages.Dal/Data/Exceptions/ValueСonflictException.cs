
namespace Messaging.Dal.Data.Exceptions
{
    public class ValueСonflictException : DataLayerException
    {
        public ValueСonflictException()
        {
        }

        public ValueСonflictException(string message) : base(message)
        {
        }

        public ValueСonflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
