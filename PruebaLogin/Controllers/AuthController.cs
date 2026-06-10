using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaLogin.Models;
using PruebaLogin.Service;


namespace PruebaLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginService _loginService;
        public AuthController(StagingContext context)
        {
            _loginService = new LoginService(context);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var isValid = await _loginService.Access(login.Username, login.Password);

            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new { message = "Success!" });
        }
    }
}
