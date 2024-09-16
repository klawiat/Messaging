using AutoMapper;
using Messaging.Api.Controllers.Base;
using Messaging.Api.Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Net;

namespace Messaging.Api.Controllers
{
    public class ErrorController : BaseController<ErrorController>
    {
        public ErrorController(IMapper mapper, ILogger<ErrorController> logger) : base(mapper, logger)
        {
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult Index(Exception ex)
        {

            ProblemDetails problemDetails = ErrorConverter(ex);
            _logger.LogError(ex, problemDetails.Detail);
            return StatusCode((int)problemDetails.Status!, problemDetails);
        }
        private ProblemDetails ErrorConverter(Exception ex)
        {
            switch (ex)
            {
                case ValueСonflictException exception:
                    return new ProblemDetails()
                    {
                        Title = "Конфликт индексов",
                        Status = (int)HttpStatusCode.Conflict,
                        Detail = exception.Message,
                        Type = "https://httpstatuses.com/409"
                    };
                case DataLayerException exception:
                    return new ProblemDetails()
                    {
                        Title = "Ошибка при работе с данными",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = exception.Message,
                        Type = "https://httpstatuses.com/500"
                    };
                case NpgsqlException exception:
                    return new ProblemDetails()
                    {
                        Title = "Ошибка при обращении к базе данных",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = exception.Message,
                        Type = "https://httpstatuses.com/500"
                    };
                case Exception exception:
                    return new ProblemDetails()
                    {
                        Title = "Неизвестная ошибка",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = exception.Message,
                        Type = "https://httpstatuses.com/500"
                    };
                default:
                    return new ProblemDetails()
                    {
                        Title = "Неизвестная ошибка",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = ex.Message,
                        Type = "https://httpstatuses.com/500"
                    };
            }
        }
    }
}