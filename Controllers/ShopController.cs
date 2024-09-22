using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_backend.Data_Structures;
using Microsoft.AspNetCore.Cors;

namespace Thesis_backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ThesisControllerBase
    {
        private readonly ILogger<ShopController> _logger;

        public ShopController(ThesisDbContext database, ILogger<ShopController> logger) : base(database)
        {
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllShopItems()
        {
            var shopItems = await Database.Shop.AllAsync;

            if (shopItems is null)
            {
                return NotFound("Can't fetch the shop items");
            }

            return Ok(shopItems.Select(x => x.Serialize));
        }

        [HttpPatch("Buy/{ID}")]
        public async Task<IActionResult> BuyShopItem(int ID)
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }
            var shopItem = await Database.Shop.Get(ID);

            if (shopItem is null)
            {
                return NotFound("Can't fetch the shop item");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);

            var game = await Database.Games.All.Include(r => r.User).SingleOrDefaultAsync(x => x.UserId == loggedInUserId);
            if (game is null)
            {
                return NotFound("No game for the user");
            }

            if (game.Currency < shopItem.Cost)
            {
                return BadRequest("Not enough money to buy this");
            }

            game.OwnedCars!.Add(new OwnedCar { GameId = game.ID, ShopId = shopItem.ID });
            if (!await Create(game.OwnedCars.Last()))
            {
                return BadRequest("Couldn't create the ownedCar record");
            }
            game.Currency -= shopItem.Cost;

            if (!await Update(game))
            {
                return BadRequest("Can't update the game's currency");
            }

            return Ok(game.Serialize);
        }
    }
}