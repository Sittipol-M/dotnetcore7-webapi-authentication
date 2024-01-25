using Microsoft.AspNetCore.Mvc;

namespace dotnetcore7_webapi_authentication.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}