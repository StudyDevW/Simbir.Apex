using Microsoft.AspNetCore.Mvc;
using Middleware_Components.Services;
using ServiceUI.Interfaces;

namespace ServiceUI.Controllers
{
    [Route("api/User/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseService _database;
        private readonly IJwtService _jwt;
        private readonly ICacheService _cache;
        private readonly ILogger _logger;

        public UsersController(IDatabaseService database, IJwtService jwt, ICacheService cache, IConfiguration configuration) 
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ServiceUI | controller-logger");
            _database = database;
            _jwt = jwt;
            _cache = cache;
        }

        //Руководитель
        [HttpPost("Add")]
        public async Task<IActionResult> AddUser(/**/)
        {
            return Ok();
        }

        [HttpPatch("Change")]
        public async Task<IActionResult> ChangeUser(/**/)
        {
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser(/**/)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id_user)
        {
            return Ok();
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetUsers(/**/)
        {
            return Ok();
        }
    }
}
