using Microsoft.AspNetCore.Mvc;

namespace SimpleAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "")]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
