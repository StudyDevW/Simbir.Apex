using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware_Components.DTO;
using Middleware_Components.Interfaces;
using ServiceUI.Interfaces;

namespace ServiceUI.Controllers
{

 
    [Route("api/User/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Asymmetric")]
    public class UsersController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ILogger _logger;
        private readonly IUIService _serviceUI;

        public UsersController(IJwtService jwt, IUIService serviceUI) 
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ServiceUI | controller-logger");
            _jwt = jwt;
            _serviceUI = serviceUI;
        }

        //ТОЛЬКО Руководитель 
        [HttpPost("Add/{email}")]
        public async Task<IActionResult> AddUser([FromBody] UserAddDTO dtoObj, string email)
        {
            try
            {
                await _serviceUI.AddNewUser(dtoObj, email, Request.Headers["Authorization"]);
                return Ok("user_added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //ТОЛЬКО Руководитель 
        [HttpPatch("Change/{id}")]
        public async Task<IActionResult> ChangeUser([FromBody] UserChangeDTO dtoObj, Guid id)
        {
            try
            {
                await _serviceUI.ChangeUser(dtoObj, id, Request.Headers["Authorization"]);
                return Ok("user_changed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //ТОЛЬКО Руководитель 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _serviceUI.DeleteUser(id, Request.Headers["Authorization"]);
                return Ok("user_deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _serviceUI.GetUser(id, Request.Headers["Authorization"]);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetUsers([FromQuery] int from, [FromQuery] int count)
        {
            try
            {
                var users = await _serviceUI.GetAllUsers(from, count, Request.Headers["Authorization"]);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
