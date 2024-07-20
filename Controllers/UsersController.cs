using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Thesis_backend.Data_Structures;

namespace Thesis_backend.Controllers
{
    public struct RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ThesisControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public UsersController(ThesisDbContext database, ILogger<WeatherForecastController> logger) : base(database)
        {
            _logger = logger;
        }

        private async Task<User?> GetUser(long userId)
        {
            return await Get<User>(userId);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            User newUser = new User()
            {
                Username = request.UserName,
                Email = request.Email,
                LastLoggedIn = DateTime.Now,
                Registered = DateTime.Now,
                PasswordHash = request.Password
            };

            if (!await Create(newUser))
            {
                return Conflict("Already exists");
            }

            return Created();
        }
    }
}