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
            return Ok(response);
        }
        [HttpPost("refresh-access-token")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenBodyRequest bodyRequest)
        {
            var response = await _authService.RefreshAccessToken(bodyRequest);
            Response.Cookies.Append("refresh-token", response.RefreshToken, new CookieOptions()
            {
                HttpOnly = true
            });
            return Ok(response);
        }
    }
}