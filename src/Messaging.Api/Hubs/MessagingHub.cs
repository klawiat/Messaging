using Messaging.Models.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace Messaging.Api.Hubs
{
    public class MessagingHub : Hub
    {
        public async Task Send(MessageViewModel messageViewModel)
        {
            await this.Clients.All.SendAsync("Receive", messageViewModel);
        }
    }
}
