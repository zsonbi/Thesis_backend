﻿using Microsoft.AspNetCore.Mvc;
using Thesis_backend.Data_Structures;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

namespace Thesis_backend.Controllers
{
    [EnableCors]
    [ApiController]
    public class ThesisControllerBase : ControllerBase
    {
        protected readonly ThesisDbContext Database;

        public long? GetLoggedInUserId => Convert.ToInt64(HttpContext.Session.GetString("UserId"));

        public ThesisControllerBase(ThesisDbContext database)
        {
            Database = database;
        }

        protected long? GetLoggedInUser()
        {
            long.TryParse(HttpContext.Session.GetString("UserId"), out long userId);
            return userId;
        }

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

        protected async Task<T?> Get<T>(string id) where T : DbElement
        {
            long.TryParse(id, out long parsedId);
            try
            {
                return await Database.GetTableManager<T>().Get(parsedId);
            }
            catch
            {
                return null;
            }
        }

        protected bool CheckUserLoggedIn()
        {
            return HttpContext.Session.GetString("UserId") is not null;
        }

        protected async Task<bool> Create<T>(T instance) where T : DbElement
        {
            try
            {
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

        protected async Task<T?> Update<T>(string id, Func<T, T?> updateRule) where T : DbElement
        {
            T? instance = await Get<T>(id);
            if (instance is null)
            {
                return null;
            }

            return await Update(instance, updateRule);
        }

        protected async Task<T?> Update<T>(T instance, Func<T, T?> updateRule) where T : DbElement
        {
            T? updated = updateRule(instance);
            if (updated is null
                || updated.ID != instance.ID)
            {
                return null;
            }

            if (!await Update(updated))
            {
                return null;
            }

            return updated;
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
    }
}