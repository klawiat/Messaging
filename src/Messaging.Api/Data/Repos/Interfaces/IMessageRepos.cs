using Messaging.Api.Models.Entities;

namespace Messaging.Api.Data.Repos.Interfaces
{
    public interface IMessageRepos
    {
        int Add(Message message);
        Message Find(int id);
        IEnumerable<Message> FindMessagesAfter(DateTime dateTime);
    }
}
