using AutoMapper;
using Messages.Services.Services.Interfaces;
using Messaging.Dal.Data.Repos.Interfaces;
using Messaging.Dal.Models.Entities;
using Messaging.Models.Models.ViewModels;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Messaging.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger _logger;
        private readonly IMessageRepos _repos;
        private readonly IMapper _mapper;
        public MessageService(ILogger<MessageService> logger, IMessageRepos repos, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repos = repos;
        }
        public IEnumerable<MessageViewModel> GetLastMessages()
        {
            _logger.LogInformation("Получение сообщений за последние 10 минут");
            var messages = _mapper
                .Map<IEnumerable<MessageViewModel>>(_repos
                    .FindMessagesAfter(DateTime.UtcNow
                        .Subtract(TimeSpan.FromMinutes(10))));
            return messages;
        }

        public MessageViewModel SendMessage(SendMessageRequest messageRequest)
        {
            var message = _mapper.Map<Message>(messageRequest);
            _logger.LogInformation(JsonSerializer.Serialize(message));
            int id = _repos.Add(message);
            var viewModel = _mapper.Map<MessageViewModel>(_repos.Find(id));
            return viewModel;
        }
    }
}
