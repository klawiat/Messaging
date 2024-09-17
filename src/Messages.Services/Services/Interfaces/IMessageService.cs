using AutoMapper;
using Messaging.Dal.Data.Repos.Interfaces;
using Messaging.Models.Models.ViewModels;
using Messaging.Services.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Services.Services.Interfaces
{
    public interface IMessageService
    {
        public IEnumerable<MessageViewModel> GetLastMessages();
        public MessageViewModel SendMessage(SendMessageRequest messageRequest);
    }
}
