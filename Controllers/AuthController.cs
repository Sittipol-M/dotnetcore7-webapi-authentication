using dotnetcore7_webapi_authentication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace dotnetcore7_webapi_authentication.Controllers
{
    [ApiController]
    [Route("authen")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterBodyRequest bodyRequest)
        {
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