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

            return CreatedAtAction(nameof(GetFriend), new { id = reciever.ID }, newFriend.Serialize);



        }


        [HttpGet("{ID}")]
        public async Task<IActionResult> GetFriend(string ID)
        {
            long convertedID;
            if (!long.TryParse(ID, out convertedID))
            {
                return NotFound("Incorrect ID format");
            }

            Data_Structures.Friend? friend = await Database.Friends.Get(convertedID);
            if (friend is null)
            {
                return NotFound("No friend with the following id");
            }
            return Ok(friend.Serialize);
        }

    }
}