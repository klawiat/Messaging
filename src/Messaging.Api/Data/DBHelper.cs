using Npgsql;

namespace Messaging.Api.Data
{
    public class DBHelper
    {
        public static void Initialize(string connectionString)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"CREATE TABLE IF NOT EXISTS MESSAGES (" +
                    "id NOT NULL auto_increment, " +
                    "Text varchar(128) NOT NULL, " +
                    "DateTime without time zone, " +
                    "Number integer NOT NULL" +
                    "PRIMARY KEY (username));" +
                    "CREATE UNIQUE INDEX IF NOT EXISTS MessageNumberIndex ON MESSAGES (Number)";
                Console.WriteLine(command.CommandText);
                command.ExecuteNonQuery();
            };

        }
    }
}
