using Microsoft.Net.Http.Headers;

namespace Messaging.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages().AddRazorPagesOptions(conf =>
            {
                conf.Conventions.AddPageRoute("/Index", "/");
            });
            //string apiUrl = "http://localhost:"+ builder.Configuration["BACKEND_HTTP_PORT_LOCAL"]?.ToString() ?? "80";
            string ahiHost = builder.Configuration["BACKEND_HOST"]?.ToString().TrimEnd('/') ?? "localhost";
            string apiPort = builder.Configuration["BACKEND_HTTP_PORT_LOCAL"]?.ToString() ?? "5004";
            builder.Services.AddHttpClient("Messaging.Api", client =>
            {
                client.BaseAddress =
                    new Uri($"http://{ahiHost}:{apiPort}");
                client.DefaultRequestHeaders.Add(
                    HeaderNames.UserAgent, "MessagingWeb");
            });

            var app = builder.Build();
            app.UseExceptionHandler("/Error");
            app.Logger.LogWarning($"http://{ahiHost}:{apiPort}");
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            app.Run();
        }
    }
}
