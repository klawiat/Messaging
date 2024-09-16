using AutoMapper;
using Messaging.Api.Controllers.Base;
using Messaging.Api.Data.Repos.Interfaces;
using Messaging.Api.Hubs;
using Messaging.Api.Mapping;
using Messaging.Api.Models.Entities;
using Messaging.Api.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Messaging.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class MessageController : BaseController<MessageController>
    {
        private readonly IMessageRepos _repos;
        public MessageController(ILogger<MessageController> logger, IMessageRepos repos, IMapper mapper) : base(mapper, logger)
        {
            _repos = repos;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            _logger.LogInformation("Получение сообщений за последние 10 минут");
            var messages = _mapper
                .Map<IEnumerable<MessageViewModel>>(_repos
                    .FindMessagesAfter(DateTime.UtcNow
                        .Subtract(TimeSpan.FromMinutes(10))));
            return Ok(messages);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Send(SendMessageRequest messageRequest, IHubContext<MessagingHub> hubContext)
        {
            _logger.LogInformation("Новое сообщение");
            var message = _mapper.Map<Message>(messageRequest);
            int id = _repos.Add(message);
            var viewModel = _mapper.Map<MessageViewModel>(_repos.Find(id));
            _logger.LogInformation("Рассылка сообщения");
            _logger.LogInformation(JsonSerializer.Serialize(hubContext.Clients));
            await hubContext.Clients.All.SendAsync("Receive", viewModel);
            return new StatusCodeResult(204);
        }
        public class SendMessageRequest : IMapWith<Message>
        {
            [Required]
            [StringLength(128)]
            public string Text { get; set; }
            public int Number { get; set; }
        }
    }
}
