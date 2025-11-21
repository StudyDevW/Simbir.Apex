using Microsoft.AspNetCore.Mvc;
using Middleware_Components.DTO;
using Middleware_Components.DTO.Enums;
using Middleware_Components.Interfaces;
using ServiceUI.Interfaces;
using static StackExchange.Redis.Role;

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

        [HttpPost("Status/{alertId}")]
        public async Task<IActionResult> AlertStatusF(Guid alertId)
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

        [HttpGet("All/{filterStatus}")]
        public async Task<IActionResult> GetAlerts(string filterStatus, [FromQuery] string filterSeverity, [FromQuery] int from, [FromQuery] int count)
        {
            try
            {
       
                if (Enum.TryParse(filterStatus, true, out AlertStatus statusOut) && Enum.TryParse(filterSeverity, true, out SeverityStatus severityOut))
                {
                    var rules = await _serviceUI.GetAlertsFromDB(new AlertsArgsDTO()
                    {
                        from = from,
                        count = count,
                        filterStatus = statusOut,
                        filterSeverity = severityOut
                    }, Request.Headers["Authorization"]);

                    _logger.LogWarning($"GetAlerts: \n  stat: {filterStatus};\n  sev: {filterSeverity};");

                    return Ok(rules);
                }
                else 
                    return BadRequest("filters_unknown");
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
