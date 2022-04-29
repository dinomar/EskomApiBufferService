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
        public JsonResult GetStatus() // -1 = error or not avaliable.
        {
            _logger.LogInformation($"[GetStatus] api call from {HttpContext.Connection.RemoteIpAddress}");

            Status status = _bufferService?.MostRecentStatus;
            return new JsonResult((status != null) ? status.Level : -1);
        }

        [HttpGet]
        public JsonResult GetStatusLog()
        {
            _logger.LogInformation($"[GetStatusLog] api call from {HttpContext.Connection.RemoteIpAddress}");

            Status status = _bufferService.MostRecentStatus;

            return (status != null) ? new JsonResult(status) : new JsonResult(new { });
        }

        [HttpGet]
        public JsonResult GetAllStatusLogs()
        {
            _logger.LogInformation($"[GetAllStatusLogs] api call from {HttpContext.Connection.RemoteIpAddress}");

            Status[] statuses = _bufferService.Statuses;
            return new JsonResult(statuses);
        }

        [HttpGet]
        public JsonResult GetLastUpdateTime()
        {
            _logger.LogInformation($"[GetLastUpdateTime] api call from {HttpContext.Connection.RemoteIpAddress}");

            Status status = _bufferService.MostRecentStatus;

            return (status != null) ? new JsonResult(status.Updated) : new JsonResult(new { });
        }

    }
}
