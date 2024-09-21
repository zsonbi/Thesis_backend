using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_backend.Data_Structures;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Thesis;
using Microsoft.AspNetCore.Cors;

namespace Thesis_backend.Controllers
{
    public record UserCreateRequest
    {
        public required string? UserName { get; set; }
        public required string? Email { get; set; }
        public required string? Password { get; set; }
    }

    public record UserLoginRequest
    {
        public required string UserIdentification { get; set; }
        public required string Password { get; set; }
    }

    [EnableCors]
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
            User? loggedInUser = await Get<User>(Convert.ToInt64(storedUserId));
            if (loggedInUser is null)
            {
                return NotFound("Not logged in");
            }
            return Ok(loggedInUser.Serialize);
        }

        [HttpDelete("Logout")]
        public IActionResult Logout()
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }
            HttpContext.Session.Remove("UserId");

            return Ok("Logged out");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            User? user = await Database.Users.All.Include(u => u.UserSettings).Include(g=>g.Game).SingleOrDefaultAsync(x => x.Username == request.UserIdentification || x.Email == request.UserIdentification);
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
        public async Task<IActionResult> Register([FromBody] UserCreateRequest request)
        {
            User newUser = new User()
            {
                Username = request.UserName,
                Email = request.Email,
                LastLoggedIn = DateTime.UtcNow,
                Registered = DateTime.UtcNow,
                PasswordHash = Crypto.HashPassword(request.Password),
                Currency = 0,
                TotalScore = 0,
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

            Game game = new Game() { Lvl = 0, NextLVLXP = 50, Currency = 0, User = newUser, UserId = newUser.ID, CurrentXP = 0, OwnedCars = new List<OwnedCar>() };
            if (!await Create(game))
            {
                return Conflict("Already exists such Game for this user");
            }

            newUser.Game = game;
            newUser.UserSettings = userSettings;

            HttpContext.Session.SetString("UserId", newUser.ID.ToString());

            return CreatedAtAction(nameof(GetLoggedInUser), newUser.Serialize);
        }
    }
}