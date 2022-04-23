using EskomApiBufferService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EskomApiBufferServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EskomController : ControllerBase
    {
        private readonly ILogger<EskomController> _logger;
        private readonly EskomBufferService _bufferService;

        public EskomController(ILogger<EskomController> logger, EskomBufferService eskomBufferService)
        {
            _logger = logger;
            _bufferService = eskomBufferService;
        }

        [HttpGet]
        public string GetStatus()
        {
            return _bufferService?.MostRecentStatus?.Level ?? string.Empty;
        }

        // GetStatusLog
        // GetAllStatusLogs
    }
}
