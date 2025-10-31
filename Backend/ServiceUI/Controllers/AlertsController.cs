using Microsoft.AspNetCore.Mvc;
using Middleware_Components.Services;
using ServiceUI.Interfaces;

namespace ServiceUI.Controllers
{
    [Route("api/Alerts/")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ILogger _logger;
        private readonly IUIService _serviceUI;

        public AlertsController(IJwtService jwt, IUIService serviceUI)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ServiceUI | controller-logger");
            _jwt = jwt;
            _serviceUI = serviceUI;
        }

        [HttpPost("Status/{id}")]
        public async Task<IActionResult> AlertStatus(Guid id)
        {

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlert(Guid id)
        {

            return BadRequest();
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAlerts()
        {

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> AlertAdminDelete(Guid id)
        {

            return BadRequest();
        }

    }
}
