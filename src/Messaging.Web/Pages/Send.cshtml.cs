using Messaging.Models.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Messaging.Web.Pages
{
    public class SendModel : PageModel
    {
        private HttpClient _httpClient;
        [BindProperty]
        public SendMessageRequest Request { get; set; }
        private ILogger<SendModel> _logger;
        public SendModel(IHttpClientFactory factory, ILogger<SendModel> logger)
        {
            _httpClient = factory.CreateClient("Messaging.Api");
            _logger = logger;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new PageResult();
            var responce = await _httpClient.PostAsJsonAsync<SendMessageRequest>("Message/Send", Request);
            _logger.LogInformation(JsonSerializer.Serialize(responce.RequestMessage.RequestUri));
            if (responce.IsSuccessStatusCode || responce.StatusCode == HttpStatusCode.NoContent)
            {
                return new RedirectResult("/Send");
            }

            var problem = await responce.Content.ReadFromJsonAsync<ProblemDetails>();

            if (responce.StatusCode == HttpStatusCode.Conflict || responce.StatusCode == HttpStatusCode.BadRequest)
                ModelState.AddModelError("", "����� ����� ��� �����");
            else
                ModelState.AddModelError("", problem.Title);
            return new PageResult();
        }
    }
}