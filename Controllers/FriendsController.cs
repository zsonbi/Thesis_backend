using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_backend.Data_Structures;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Thesis;
using Microsoft.AspNetCore.Cors;
using Castle.Core.Smtp;

namespace Thesis_backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ThesisControllerBase
    {
        private readonly ILogger<FriendsController> _logger;

        public FriendsController(ThesisDbContext database, ILogger<FriendsController> logger) : base(database)
        {
            _logger = logger;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendFriendRequest([FromBody] string UserIdentification)
        {
            string? storedUserId = HttpContext.Session.GetString("UserId");
            if (storedUserId is null)
            {
                return NotFound("Not logged in");
            }

            User? reciever = await Database.Users.All.Include(u => u.UserSettings).SingleOrDefaultAsync(x => x.Username == UserIdentification || x.Email == UserIdentification);

            if (reciever is null)
            {
                return NotFound("No such user exist");
            }
            if (storedUserId == reciever.ID.ToString())
            {
                return BadRequest("Can't send friend request to yourself");
            }

            Friend newFriend = new Friend()
            {
                Pending = true,
                Reciever = reciever,
                Sender = Database.Users.Get(Convert.ToInt64(storedUserId)).Result,
                SentTime = DateTime.Now,
            };

            if (!await Create(newFriend))
            {
                return Conflict("Can't send friend request to him");
            }

            return CreatedAtAction(nameof(GetFriend), new { id = newFriend.ID }, newFriend.Serialize);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetFriendByID(string ID)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long convertedID;
            if (!long.TryParse(ID, out convertedID))
            {
                return NotFound("Incorrect ID format");
            }

            Data_Structures.Friend? friend = await Database.Friends.All.Include(r => r.Reciever).Include(s => s.Sender).SingleOrDefaultAsync(x => x.ID == convertedID);

            if (friend is null)
            {
                return NotFound("No friend with the following id");
            }
            if (!(friend.Sender!.ID == GetLoggedInUser() || friend.Reciever!.ID == GetLoggedInUser()))
            {
                return BadRequest("Not releated to you");
            }

            return Ok(friend.Serialize);
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> GetFriend(string ID)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long convertedID;
            if (!long.TryParse(ID, out convertedID))
            {
                return NotFound("Incorrect ID format");
            }

            Data_Structures.Friend? friend = await Database.Friends.All.Include(r => r.Reciever).Include(s => s.Sender).SingleOrDefaultAsync(x => x.ID == convertedID);

            if (friend is null)
            {
                return NotFound("No friend with the following id");
            }
            if (!(friend.Sender!.ID == GetLoggedInUser() || friend.Reciever!.ID == GetLoggedInUser()))
            {
                return BadRequest("Not releated to you");
            }

            return Ok(friend.Serialize);
        }
    }
}