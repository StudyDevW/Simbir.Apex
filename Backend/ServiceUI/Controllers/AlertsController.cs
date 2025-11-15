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

        [HttpPost("Status/{alertId}/(NOTREADY)")]
        public async Task<IActionResult> AlertStatus(Guid alertId)
        {
            return BadRequest();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlert(Guid id)
        {
            try
            {
                var rules = await _serviceUI.GetAlertFromDB(id, Request.Headers["Authorization"]);
                return Ok(rules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAlerts([FromQuery] int from, [FromQuery] int count)
        {
            try
            {
                var rules = await _serviceUI.GetAlertsFromDB(from, count, Request.Headers["Authorization"]);
                return Ok(rules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/(NOTREADY)")]
        public async Task<IActionResult> AlertAdminDelete(Guid id)
        {

            return BadRequest();
        }

    }
}
