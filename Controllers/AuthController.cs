using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("google/callback-success")]
        public async Task GoogleLogin()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var name = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var returnUrl = HttpContext.Request.Query["returnUrl"].ToString();

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync("Invalid returnUrl");

            }

            if (!(HttpContext.User.Identity?.IsAuthenticated ?? false))
            {
                HttpContext.Response.Redirect($"{returnUrl}?error=auth_failed");
            }


            var token = "_JWT";

            var redirectUrl =
                $"{returnUrl}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email ?? "")}&name={Uri.EscapeDataString(name ?? "")}";

            HttpContext.Response.Redirect(redirectUrl);

        }


        [HttpGet("login")]
        public async Task<IActionResult> Validate()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var name = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("http://localhost:4200");
        }
    }
}
