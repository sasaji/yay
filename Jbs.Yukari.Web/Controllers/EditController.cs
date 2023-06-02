using System.Reflection;
using System.Runtime.CompilerServices;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public abstract class EditController<T> : Controller where T : BasicInfo
    {
        protected readonly ILogger<EditController<T>> _logger;
        protected readonly IQuery _query;
        protected readonly IRomanizer _romanizer;

        public EditController(ILogger<EditController<T>> logger, IQuery query, IRomanizer romanizer)
        {
            _logger = logger;
            _query = query;
            _romanizer = romanizer;
        }

        public async Task<T> Get(string yid)
        {
            var model = await _query.Get<T>(yid);
            model.Users = await _query.GetObjects<User>(yid, "user");
            model.Groups = await _query.GetObjects<Group>(yid, "group");
            return model;
        }

        public virtual IActionResult Save(T model)
        {
            try
            {
                ViewData["Result"] = "0";
            }
            catch (Exception ex)
            {
                ViewData["Result"] = "1";
                ViewData["ErrorMessage"] = ex.Message;
            }
            return View("Index", model);
        }
    }
}
