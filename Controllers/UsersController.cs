using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_backend.Data_Structures;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Thesis;

namespace Thesis_backend.Controllers
{
    public struct UserRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ThesisControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ThesisDbContext database, ILogger<UsersController> logger) : base(database)
        {
            _logger = logger;
        }

        [HttpGet("LoggedInUser")]
        public async Task<IActionResult> GetLoggedInUser()
        {
            string? storedUserId = HttpContext.Session.GetString("UserId");
            if (storedUserId is null)
            {
                return NotFound("Not logged in");
            }

            return OkOrNotFound<User>(await Get<User>(Convert.ToInt64(storedUserId)));
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            if (CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }
            HttpContext.Session.Remove("UserId");

            return Ok("Logged out");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserRequest request)
        {
            User? user = await Database.Users.All.Include(u => u.UserSettings).SingleOrDefaultAsync(x => x.Username == request.UserName || x.Email == request.Email);
            if (user == null)
            {
                return NotFound("Can't find user with this email or password");
            }

            if (!Crypto.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Incorrect password");
            }
            HttpContext.Session.SetString("UserId", user.ID.ToString());

            return Ok(user?.Serialize);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            User newUser = new User()
            {
                Username = request.UserName,
                Email = request.Email,
                LastLoggedIn = DateTime.Now,
                Registered = DateTime.Now,
                PasswordHash = Crypto.HashPassword(request.Password)
            };

            if (!await Create(newUser))
            {
                return Conflict("Already exists");
            }

            UserSettings userSettings = new UserSettings() { User = newUser, privacy = 0, ProfilePic = "", UserId = newUser.ID };

            if (!await Create(userSettings))
            {
                return Conflict("Already exists such UserSettings for this user");
            }

            //TODO add gameID

            HttpContext.Session.SetString("UserId", newUser.ID.ToString());

            return Created();
        }
    }
}