using Messaging.Dal.Data.Exceptions;
using Messaging.Dal.Data.Repos.Base;
using Messaging.Dal.Data.Repos.Interfaces;
using Messaging.Dal.Models.Entities;
using Npgsql;

namespace Messaging.Dal.Data.Repos
{
    public class MessageRepos : BaseRepo, IMessageRepos
    {
        public MessageRepos(NpgsqlConnection connection) : base(connection)
        {
            _connection.Open();
        }

        /*public MessageRepos(string conStr) : base(conStr)
        {
            _connection.Open();
        }*/

        public int Add(Message message)
        {
            try
            {
                var command = _connection.CreateCommand();
                command.CommandText = @"INSERT INTO MESSAGES (Text,DateTime,Number) VALUES (@text, @date, @number) RETURNING id;";
                NpgsqlParameter text = new NpgsqlParameter("@text", message.Text);
                command.Parameters.Add(text);
                NpgsqlParameter dateTime = new NpgsqlParameter("@date", DateTime.UtcNow);
                command.Parameters.Add(dateTime);
                NpgsqlParameter number = new NpgsqlParameter("@number", message.Number);
                command.Parameters.Add(number);
                NpgsqlParameter id = new NpgsqlParameter()
                {
                    ParameterName = "@id",
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer,
                    Direction = System.Data.ParameterDirection.Output
                };
                command.Parameters.Add(id);
                command.ExecuteNonQuery();
                return Convert.ToInt32(id.Value);
            }
            catch (NpgsqlException ex)
            {
                if (ex.ErrorCode == 23505 || ex.Message.Contains("messagenumberindex"))
                    throw new ValueСonflictException("Совпадают номера сообщений");
                throw new CommandExecutionException("Произошла ошибка при добавлении сообщения", ex);
            }
            catch (Exception ex)
            {
                throw new DataLayerException("Произошла ошибка", ex);
            }
        }

        public Message Find(int id)
        {
            try
            {
                var command = _connection.CreateCommand();
                command.CommandText = @"SELECT id,Text,DateTime,Number FROM MESSAGES WHERE id=@id";
                NpgsqlParameter identifier = new NpgsqlParameter("@id", id);
                command.Parameters.Add(identifier);
                var reader = command.ExecuteReader();
                Message message = new();
                if (reader.HasRows)
                {
                    reader.Read();
                    message.Id = reader.GetInt32(0);
                    message.Text = reader.GetString(1);
                    message.DateTime = reader.GetDateTime(2);
                    message.Number = reader.GetInt32(3);
                }
                return message;
            }
            catch (NpgsqlException ex)
            {
                throw new CommandExecutionException("Произошла ошибка при извлечении данных из базы", ex);
            }
            catch (Exception ex)
            {
                throw new DataLayerException("Произошла неизвестная ошибка", ex);
            }
        }

        public IEnumerable<Message> FindMessagesAfter(DateTime dateTime)
        {
            try
            {
                var command = _connection.CreateCommand();
                command.CommandText = @"SELECT id,Text,DateTime,Number FROM MESSAGES WHERE DateTime>=@date";
                NpgsqlParameter dateTimeParameter = new NpgsqlParameter("@date", dateTime);
                command.Parameters.Add(dateTimeParameter);
                var reader = command.ExecuteReader();
                List<Message> messages = new List<Message>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string text = reader.GetString(1);
                        DateTime dateTimeMessage = reader.GetDateTime(2);
                        int number = reader.GetInt32(3);
                        messages.Add(
                            new Message
                            {
                                Id = id,
                                Text = text,
                                DateTime = dateTimeMessage,
                                Number = number
                            });
                    }
                }
                return messages;
            }
            catch (NpgsqlException ex)
            {
                throw new CommandExecutionException("Произошла ошибка при извлечении данных из базы", ex);
            }
            catch (Exception ex)
            {
                throw new DataLayerException("Произошла неизвестная ошибка", ex);
            }
        }
    }
}
