﻿using Microsoft.AspNetCore.Mvc;
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
            return Ok(game);
        }
    }
}