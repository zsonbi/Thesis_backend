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
    public class ShopController : ThesisControllerBase
    {
        private readonly ILogger<ShopController> _logger;

        public ShopController(ThesisDbContext database, ILogger<ShopController> logger) : base(database)
        {
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetFriend()
        {
            var shopItems = await Database.Shop.AllAsync;

            if (shopItems is null)
            {
                return NotFound("Can't fetch the shop items");
            }

            return Ok(shopItems.Select(x => x.Serialize));
        }
    }
}