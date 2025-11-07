using Microsoft.AspNetCore.Mvc;
using Middleware_Components.Services;
using ServiceUI.Interfaces;

namespace ServiceUI.Controllers
{
    [Route("api/Reports/")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ILogger _logger;
        private readonly IUIService _serviceUI;

        public ReportsController(IJwtService jwt, IUIService serviceUI)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ServiceUI | controller-logger");
            _jwt = jwt;
            _serviceUI = serviceUI;
        }

        [HttpPost("Add/(NOTREADY)")]
        public async Task<IActionResult> AddReport()
        {

            return BadRequest();
        }

        [HttpPatch("Change/(NOTREADY)")]
        public async Task<IActionResult> ChangeReport()
        {

            return BadRequest();
        }

        [HttpDelete("Delete/{id}/(NOTREADY)")]
        public async Task<IActionResult> DeleteReport(Guid id)
        {

            return BadRequest();
        }


        [HttpGet("{id}/(NOTREADY)")]
        public async Task<IActionResult> GetReport(Guid id)
        {

            return BadRequest();
        }

        [HttpGet("All/(NOTREADY)")]
        public async Task<IActionResult> GetReports()
        {

            return BadRequest();
        }
    }
}
