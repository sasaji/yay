using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Jbs.Yukari.Web.Controllers
{
    public abstract class EditController<T> : Controller where T : BasicInfo
    {
        protected readonly ILogger<EditController<T>> _logger;
        protected readonly ISql _sql;
        protected readonly IRomanizer _romanizer;
        protected readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public EditController(ILogger<EditController<T>> logger, ISql sql, IRomanizer romanizer)
        {
            _logger = logger;
            _sql = sql;
            _romanizer = romanizer;
        }

        public async Task<T> Get(Guid yid)
        {
            var model = await _sql.Get<T>(yid);
            model.Roles = await _sql.GetRole(yid);
            model.Users = await _sql.GetObjects<User>(yid, "user");
            model.Groups = await _sql.GetObjects<Group>(yid, "group");
            return model;
        }

        public virtual IActionResult Save(T model)
        {
            try
            {
                model.Name = BuildName(model);
                _sql.Save(model);
                model.Phase = 2;
                ViewData["Result"] = "0";
            }
            catch (Exception ex)
            {
                ViewData["Result"] = "1";
                ViewData["ErrorMessage"] = ex.Message;
            }
            return View("Index", model);
        }

        public virtual IActionResult Publish(T model)
        {
            try
            {
                _sql.Publish(model.Yid);
                model.Phase = 0;
                ViewData["Result"] = "0";
            }
            catch (Exception ex)
            {
                ViewData["Result"] = "1";
                ViewData["ErrorMessage"] = ex.Message;
            }
            return View("Index", model);
        }

        protected virtual string BuildName(T model)
        {
            return model.Name;
        }
    }
}
