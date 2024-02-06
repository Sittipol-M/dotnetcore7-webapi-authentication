using dotnetcore7_webapi_authentication.Requests;
using dotnetcore7_webapi_authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnetcore7_webapi_authentication.Controllers
{
    [ApiController]
    [Route("authen")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterBodyRequest bodyRequest)
        {
            await _authService.Register(bodyRequest);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginBodyRequest bodyRequest)
        {
            var response = await _authService.Login(bodyRequest);
            Response.Cookies.Append("refresh-token", response.RefreshToken, new CookieOptions()
            {
                HttpOnly = true
            });
            Response.Cookies.Append("access-token", response.AccessToken, new CookieOptions()
            {
                HttpOnly = true
            });
            return Ok(response);
        }
        [HttpPost("refresh-access-token")]
        public async Task<IActionResult> RefreshAccessToken()
        {
            var refreshToken = Request.Cookies["refresh-token"];
            var accessToken = Request.Cookies["access-token"];
            var response = await _authService.RefreshAccessToken(accessToken, refreshToken);
            Response.Cookies.Append("access-token", response.AccessToken, new CookieOptions()
            {
                HttpOnly = true
            });
            Response.Cookies.Append("refresh-token", response.RefreshToken, new CookieOptions()
            {
                HttpOnly = true
            });
            return Ok(response);
        }
    }
}