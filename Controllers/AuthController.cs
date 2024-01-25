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
        public IActionResult Register([FromBody] RegisterBodyRequest bodyRequest)
        {
            _authService.Register(bodyRequest);
            return Ok();
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginBodyRequest bodyRequest)
        {
            return Ok();
        }
        [HttpPost("refreshAccessToken")]
        public IActionResult RefreshAccessToken([FromBody] RefreshAccessTokenBodyRequest bodyRequest)
        {
            return Ok();
        }
    }
}