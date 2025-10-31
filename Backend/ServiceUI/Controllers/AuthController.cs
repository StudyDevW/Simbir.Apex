using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware_Components.DTO;
using Middleware_Components.Services;
using ServiceUI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServiceUI.Controllers
{
    [Route("api/Auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ILogger _logger;
        private readonly IUIService _serviceUI;

        public AuthController(IJwtService jwt, IUIService serviceUI)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ServiceUI | controller-logger");
            _jwt = jwt;
            _serviceUI = serviceUI; 
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInFunc([FromBody] AuthDTO dtoObj)
        {
            var auth = await _serviceUI.AuthPart(dtoObj.name, dtoObj.password);

            if (auth != null)
            {
                return Ok(auth);
            }

            return BadRequest();
        }

        [HttpGet("Validate")]
        public async Task<IActionResult> ValidateToken([Required][FromHeader(Name = "accessToken")] string? token)
        {
            if (token != null)
            {
                if (token.Contains("Bearer"))
                    return BadRequest("accessToken in this method must not contain word [Bearer]");
            }

            var validation = await _jwt.AccessTokenValidation("Bearer " + token);


            if (validation.TokenHasError())
            {
                return Unauthorized();
            }
            else if (validation.TokenHasSuccess())
            {
                _logger.LogInformation($"Токен для id: {validation.token_success.Id} валид!");
                return Ok("valid");
            }

            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = "Asymmetric")]
        [HttpPut("SignOut")]
        public async Task<IActionResult> UserSignOut()
        {
            var signInfo = await _serviceUI.ClientSignOut(Request.Headers["Authorization"]);

            if (signInfo != null)
            {
                return Ok(signInfo);
            }

            return Unauthorized();
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> UserRefreshTokens([FromBody] AuthRefreshTokens dtoObj)
        {
            var refreshInfo = await _serviceUI.RefreshClientSession(dtoObj.refreshToken);

            if (refreshInfo != null)
            {
                return Ok(refreshInfo);
            }

            return Unauthorized();
        }

    }
}
