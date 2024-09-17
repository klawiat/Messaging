namespace Messaging.Dal.Data.Exceptions
{
    public class DataLayerException : ApplicationException
    {
        public DataLayerException() { }
        public DataLayerException(string message) : base(message) { }
        public DataLayerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
