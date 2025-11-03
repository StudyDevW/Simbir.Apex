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
            try
            {
                var auth = await _serviceUI.AuthPart(dtoObj.name, dtoObj.password);

                if (auth != null)
                {
                    //Запись в куки refresh токена
                    Response.Cookies.Append("refreshToken", auth.refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    return Ok(new AuthTokenInfo() { accessToken = auth.accessToken, expires_at = auth.expires_at });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<IActionResult> UserRefreshTokens()
        {
            var refreshTokenCookies = Request.Cookies["refreshToken"];

            if (refreshTokenCookies == null)
                return Unauthorized("token_not_found");

            _logger.LogWarning($"(DEBUG) REFRESH FROM COOKIES: {refreshTokenCookies}");

            var refreshInfo = await _serviceUI.RefreshClientSession(refreshTokenCookies);

            if (refreshInfo != null)
            {
                Response.Cookies.Append("refreshToken", refreshInfo.refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Ok(new AuthTokenInfo() { accessToken = refreshInfo.accessToken, expires_at = refreshInfo.expires_at });
            }

            return Unauthorized();
        }

    }
}
