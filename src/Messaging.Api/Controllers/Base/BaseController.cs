using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Controllers.Base
{
    [ApiController]
    public abstract class BaseController<T> : Controller
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;
        protected BaseController(IMapper mapper, ILogger<T> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }
    }
}
