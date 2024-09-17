using AutoMapper;
using Messaging.Api.Controllers.Base;
using Messaging.Dal.Data.Repos.Interfaces;
using Messaging.Api.Hubs;
using Messaging.Models.Mapping;
using Messaging.Dal.Models.Entities;
using Messaging.Models.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Messages.Services.Services.Interfaces;

namespace Messaging.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class MessageController : BaseController<MessageController>
    {
        private readonly IMessageRepos _repos;
        private readonly IMessageService _service;
        public MessageController(ILogger<MessageController> logger, IMessageRepos repos, IMapper mapper, IMessageService service) : base(mapper, logger)
        {
            _repos = repos;
            _service = service;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            return Ok(_service.GetLastMessages());
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Send(SendMessageRequest messageRequest, IHubContext<MessagingHub> hubContext)
        {
            var viewModel = _service.SendMessage(messageRequest);
            _logger.LogInformation("Рассылка сообщения");
            await hubContext.Clients.All.SendAsync("Receive", viewModel);
            return new StatusCodeResult(204);
        }
    }
}
