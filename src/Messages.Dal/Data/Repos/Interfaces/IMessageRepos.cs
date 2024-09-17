using Messaging.Dal.Models.Entities;

namespace Messaging.Dal.Data.Repos.Interfaces
{
    public interface IMessageRepos
    {
        int Add(Message message);
        Message Find(int id);
        IEnumerable<Message> FindMessagesAfter(DateTime dateTime);
    }
}
