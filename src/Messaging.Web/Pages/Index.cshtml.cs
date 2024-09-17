using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Messaging.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string hubUrl { get; private set; }
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            string ahiHost = configuration["API_URL"]?.ToString().TrimEnd('/') ?? @"http://localhost";
            string apiPort = configuration["BACKEND_HTTP_PORT"]?.ToString() ?? "5004";
            hubUrl = $"{ahiHost}:{apiPort}/messages";
        }

        public void OnGet()
        {

        }
    }
}
