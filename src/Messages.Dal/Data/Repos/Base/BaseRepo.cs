using Npgsql;

namespace Messaging.Dal.Data.Repos.Base
{
    public abstract class BaseRepo
    {
        protected NpgsqlConnection _connection;
        private bool _isDisposed;
        private readonly bool _disposeConnection;
        public BaseRepo(NpgsqlConnection connection)
        {
            _connection = connection;
            _disposeConnection = false;
        }
        public BaseRepo(string conStr)
        {
            _connection = new NpgsqlConnection(conStr);
            _disposeConnection = true;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (_disposeConnection)
                {
                    _connection.Dispose();
                }
            }

            _isDisposed = true;
        }
        ~BaseRepo()
        {
            Dispose(false);
        }
    }
}
