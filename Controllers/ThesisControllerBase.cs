using Microsoft.AspNetCore.Mvc;
using Thesis_backend.Data_Structures;
using System.Text.Json;

namespace Thesis_backend.Controllers
{
    [ApiController]
    public class ThesisControllerBase : ControllerBase
    {
        protected readonly ThesisDbContext Database;

        public ThesisControllerBase(ThesisDbContext database)
        {
            Database = database;
        }

        //LogoDbContext actions

        protected async Task<T?> Get<T>(long id) where T : DbElement
        {
            try
            {
                return await Database.GetTableManager<T>().Get(id);
            }
            catch
            {
                return null;
            }
        }

        protected async Task<bool> Create<T>(T instance) where T : DbElement
        {
            try
            {
                //Create would check the CompanyHashID, but it would not be resolveable before the creation of said object
                return await Database.GetTableManager<T>().Create(instance);
            }
            catch
            {
                return false;
            }
        }

        protected async Task<bool> Update<T>(T instance) where T : DbElement
        {
            try
            {
                return await Database.GetTableManager<T>().Update(instance);
            }
            catch
            {
                return false;
            }
        }

        protected async Task<bool> Delete<T>(long id) where T : DbElement
        {
            try
            {
                return await Database.GetTableManager<T>().Delete(id);
            }
            catch
            {
                return false;
            }
        }

        protected async Task<bool> Delete<T>(T instance) where T : DbElement
        {
            try
            {
                return await Database.GetTableManager<T>().Delete(instance.ID);
            }
            catch
            {
                return false;
            }
        }

        protected IActionResult OkOrNotFound(bool t)
        {
            if (t is false)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        protected IActionResult OkOrNotFound<T>(T? t) where T : DbElement
        {
            if (t is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(JsonSerializer.Serialize<T>(t));
            }
        }

        protected string GetClaim(string claimString)
        {
            if (HttpContext is null
                || HttpContext.User is null
                || HttpContext.User.Identity is null ||
                !HttpContext.User.Identity.IsAuthenticated)
            {
                return "";
            }

            var claim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == claimString);
            if (claim == null)
            {
                return "";
            }

            return claim.Value;
        }
    }
}