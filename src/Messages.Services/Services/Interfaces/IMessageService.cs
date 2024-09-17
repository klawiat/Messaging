using Messaging.Models.Models.ViewModels;

namespace Messages.Services.Services.Interfaces
{
    public interface IMessageService
    {
        public IEnumerable<MessageViewModel> GetLastMessages();
        public MessageViewModel SendMessage(SendMessageRequest messageRequest);
    }
}
