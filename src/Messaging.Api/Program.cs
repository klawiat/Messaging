using Messaging.Dal.Data.Repos;
using Messaging.Dal.Data.Repos.Interfaces;
using Messaging.Api.Hubs;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.OpenApi.Models;
using Npgsql;
using Serilog;
using System.Reflection;
using Messaging.Dal.Models.Entities;
using Messages.Services.Services.Interfaces;
using Messaging.Services.Services;
using Messaging.Models.Models.ViewModels;

namespace Messaging.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Configuration.AddEnvironmentVariables();
                NpgsqlConnectionStringBuilder connectionBuilder = new NpgsqlConnectionStringBuilder()
                {
                    Host = builder.Configuration["POSTGRES_HOST"] ?? "localhost",
                    Port = int.Parse(builder.Configuration["POSTGRES_PORT"] ?? "5432"),
                    Username = builder.Configuration["POSTGRES_USER"] ?? "postgres",
                    Password = builder.Configuration["POSTGRES_PASSWORD"] ?? "postgres",
                    Database = builder.Configuration["POSTGRES_DB"] ?? "postgres"
                };
                #region Logging
                builder.Logging.ClearProviders();
                builder.Logging.AddSerilog();
                builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
                #endregion
                #region Services
                builder.Services.AddSignalR();
                builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(),typeof(SendMessageRequest).Assembly);
                builder.Services.AddTransient<NpgsqlConnection>((a) => new NpgsqlConnection(connectionBuilder.ConnectionString));
                builder.Services.AddTransient<IMessageRepos, MessageRepos>();
                builder.Services.AddTransient<IMessageService, MessageService>();
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(cfg =>
                {
                    cfg.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Title = "Messaging api",
                        Version = "v1"
                    });
                });
                #endregion
                var app = builder.Build();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(cfg =>
                {
                    cfg.AllowAnyOrigin();
                    cfg.AllowAnyHeader();
                    cfg.AllowAnyMethod();
                });
                app.UseExceptionHandler("/error");

                app.MapControllers();
                app.MapHub<MessagingHub>("/messages", cfg =>
                {
                    cfg.Transports = HttpTransportType.WebSockets;
                });

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
    }
}
