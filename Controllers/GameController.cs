using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_backend.Data_Structures;
using Microsoft.AspNetCore.Cors;

namespace Thesis_backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ThesisControllerBase
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ThesisDbContext database, ILogger<GameController> logger) : base(database)
        {
            _logger = logger;
        }

        [HttpPatch("DoubleCoinsForGame")]
        public async Task<IActionResult> DoubleCoinsForGame()
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }
            long loggedInUserId = (long)(this.GetLoggedInUser()!);

            var game = await Database.Games.All.Include(r => r.User).SingleOrDefaultAsync(x => x.UserId == loggedInUserId);
            if (game is null)
            {
                return NotFound("No game for the user");
            }

            if (game.User.CurrentTaskScore < Config.DOUBLE_COIN_TASK_POINT_COST)
            {
                return BadRequest("Not enough task points");
            }
            game.User.CurrentTaskScore -= Config.DOUBLE_COIN_TASK_POINT_COST;

            if (!(await Update(game.User)))
            {
                return BadRequest("Can't update the user's remaining task points");
            }

            return Ok(game.User.Serialize);
        }

        [HttpGet("GetAllOwnedCars")]
        public async Task<IActionResult> GetAllOwnedCars()
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);

            var game = await Database.Games.All
            .Include(r => r.User).SingleOrDefaultAsync(x => x.UserId == loggedInUserId);

            if (game is null)
            {
                return NotFound("No friend with releated user identification");
            }

            return Ok(game.OwnedCars?.Select(x => x.Serialize));
        }

        [HttpPost("Scores/Store")]
        public async Task<IActionResult> StoreScore([FromBody] int score)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);
            User? loggedInUser = await Database.Users.All.SingleOrDefaultAsync(x => x.ID == loggedInUserId);
            if (loggedInUser is null)
            {
                return NotFound("Can't find logged in user");
            }
            GameScore gameScore = new GameScore() { Owner = loggedInUser, AchievedTime = DateTime.UtcNow, Score = score };
            if (!await Create(gameScore))
            {
                return BadRequest("Can't create the game score record");
            }

            return Ok(gameScore.Serialize);
        }

        [HttpGet("Scores/Get")]
        public async Task<IActionResult> GetScores([FromBody] DateTime since)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            List<GameScore> gameScores = await Database.GameScores.All.Where(x => x.AchievedTime > since).OrderByDescending(x => x.Score).Take(Config.LEADERBOARD_SIZE).ToListAsync();

            if (gameScores is null)
            {
                return NotFound("Can't find the game score leaderboard");
            }

            return Ok(gameScores.Select(x => x.Serialize));
        }

        [HttpPatch("AddCoin")]
        public async Task<IActionResult> AddCoin([FromBody] int amount)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);

            var game = await Database.Games.All.Include(r => r.User).SingleOrDefaultAsync(x => x.UserId == loggedInUserId);

            if (game is null)
            {
                return NotFound("No game for the user");
            }
            game.Currency += amount;

            if (!await Update(game))
            {
                return BadRequest("Can't update the game's currency");
            }
            return Ok(game.Serialize);
        }

        [HttpPatch("PowerUp/Turbo")]
        public async Task<IActionResult> BuyTurbo()
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);
            User? loggedInUser = await Database.Users.All.SingleOrDefaultAsync(x => x.ID == loggedInUserId);

            if (loggedInUser is null)
            {
                return NotFound("Can't find logged in user");
            }
            if (loggedInUser.CurrentTaskScore < Config.TURBO_TASK_POINT_COST)
            {
                return BadRequest("Not enough task points to buy turbo");
            }
            loggedInUser.CurrentTaskScore -= Config.TURBO_TASK_POINT_COST;

            if (!await Update(loggedInUser))
            {
                return BadRequest("Couldn't update the user");
            }

            return Ok(loggedInUser.Serialize);
        }

        [HttpPatch("PowerUp/Immunity")]
        public async Task<IActionResult> BuyInvincibility()
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);
            User? loggedInUser = await Database.Users.All.SingleOrDefaultAsync(x => x.ID == loggedInUserId);

            if (loggedInUser is null)
            {
                return NotFound("Can't find logged in user");
            }
            if (loggedInUser.CurrentTaskScore < Config.INVINCIBILITY_TASK_POINT_COST)
            {
                return BadRequest("Not enough task points to buy invincibility");
            }
            loggedInUser.CurrentTaskScore -= Config.INVINCIBILITY_TASK_POINT_COST;

            if (!await Update(loggedInUser))
            {
                return BadRequest("Couldn't update the user");
            }

            return Ok(loggedInUser.Serialize);
        }
    }
}