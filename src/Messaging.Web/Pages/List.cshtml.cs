using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Messaging.Web.Pages
{
    public class ListModel : PageModel
    {
        private HttpClient _httpClient;
        public IEnumerable<MessageViewModel> Messages { get; set; }
        public ListModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("Messaging.Api");
        }
        public async Task OnGet()
        {
            var result = await _httpClient.GetAsync("Message/Get");
            if (!result.IsSuccessStatusCode)
                Messages = new List<MessageViewModel>();
            else
                Messages = (await result.Content.ReadFromJsonAsync<IEnumerable<MessageViewModel>>()) ?? new List<MessageViewModel>();
        }
        public class MessageViewModel
        {
            public string Message { get; set; }
            public DateTime DateTime { get; set; }
            public int Number { get; set; }
        }
    }
}
