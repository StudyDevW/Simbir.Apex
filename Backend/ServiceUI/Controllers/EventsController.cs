using Microsoft.AspNetCore.Mvc;
using Middleware_Components.Services;
using ServiceUI.Interfaces;

namespace ServiceUI.Controllers
{
    [Route("api/Events/")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ILogger _logger;
        private readonly IUIService _serviceUI;

        public EventsController(IJwtService jwt, IUIService serviceUI)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ServiceUI | controller-logger");
            _jwt = jwt;
            _serviceUI = serviceUI;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            try
            {
                var events = await _serviceUI.GetEventFromDB(id, Request.Headers["Authorization"]);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var events = await _serviceUI.GetEventsFromDB(Request.Headers["Authorization"]);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
