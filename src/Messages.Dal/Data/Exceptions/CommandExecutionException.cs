namespace Messaging.Dal.Data.Exceptions
{
    public class CommandExecutionException : DataLayerException
    {
        public CommandExecutionException() { }
        public CommandExecutionException(string message) : base(message) { }
        public CommandExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
