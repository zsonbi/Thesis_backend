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
                return Conflict("Can't send friend request to yourself");
            }

            Data_Structures.Friend? friend = await Database.Friends.All.Include(r => r.Receiver).Include(s => s.Sender).SingleOrDefaultAsync(x => x.Receiver!.ID == GetLoggedInUser() && x.Sender!.ID == reciever.ID);

            if (friend is not null)
            {
                return Conflict($"You can't send an another friend request to {reciever.Username}");
            }

            Friend newFriend = new Friend()
            {
                Pending = true,
                Receiver = reciever,
                Sender = Database.Users.Get(Convert.ToInt64(storedUserId)).Result,
                SentTime = DateTime.UtcNow,
            };

            if (!await Create(newFriend))
            {
                return Conflict("Can't send friend request to him");
            }

            return CreatedAtAction(nameof(GetFriend), new { id = newFriend.ID }, newFriend.Serialize);
        }

        [HttpGet("{ID}/Get")]
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

            Data_Structures.Friend? friend = await Database.Friends.All.Include(r => r.Receiver).Include(s => s.Sender).SingleOrDefaultAsync(x => x.ID == convertedID);

            if (friend is null)
            {
                return NotFound("No friend with the following id");
            }
            if (!(friend.Sender!.ID == GetLoggedInUser() || friend.Receiver!.ID == GetLoggedInUser()))
            {
                return BadRequest("Not releated to you");
            }

            return Ok(friend.Serialize);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetFriend()
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);

            var friends = await Database.Friends.All
            .Include(r => r.Receiver)
            .Include(s => s.Sender)
            .Where(x => x.Sender!.ID == loggedInUserId || x.Receiver!.ID == loggedInUserId)
            .ToArrayAsync();

            if (friends is null)
            {
                return NotFound("No friend with releated user identification");
            }

            return Ok(friends.Select(x => x.Serialize));
        }

        [HttpPatch("{ID}/Accept")]
        public async Task<IActionResult> AcceptFriendRequest(string ID)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);
            long convertedID;

            if (!long.TryParse(ID, out convertedID))
            {
                return NotFound("Incorrect ID format");
            }
            var friend = await Database.Friends.All
            .Include(r => r.Receiver)
            .Include(s => s.Sender)
            .SingleOrDefaultAsync(x => x.ID == convertedID);

            if (friend is null)
            {
                return NotFound("No friend with releated id");
            }

            if (friend.Receiver!.ID != loggedInUserId)
            {
                return NotFound("You can't accept this friend request you are not the reciever");
            }

            if (!friend.Pending)
            {
                return BadRequest("Already accepted");
            }

            friend.Pending = false;
            if (!await Update(friend))
            {
                return BadRequest("Can't update the friend request");
            }
            return Ok(friend);
        }

        [HttpDelete("{ID}/Delete")]
        public async Task<IActionResult> DeleteFriend(string ID)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);
            long convertedID;

            if (!long.TryParse(ID, out convertedID))
            {
                return NotFound("Incorrect ID format");
            }

            var friend = await Database.Friends.All
            .Include(r => r.Receiver)
            .Include(s => s.Sender)
            .SingleOrDefaultAsync(x => x.ID == convertedID);

            if (friend is null)
            {
                return NotFound("No friend with such friend id");
            }

            if (!(friend.Receiver?.ID == loggedInUserId || friend.Sender?.ID == loggedInUserId))
            {
                return NotFound("You have no such friend :C");
            }
            bool pending = friend.Pending;
            if (!await Delete(friend))
            {
                return BadRequest("Can't reject the friend request");
            }

            return Ok("Deleted");
        }
    }
}