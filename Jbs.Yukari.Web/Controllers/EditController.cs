using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public abstract class EditController<T> : Controller where T : BasicInfo
    {
        protected readonly ILogger<EditController<T>> _logger;
        protected readonly ISql _sql;
        protected readonly IRomanizer _romanizer;
        protected readonly IJsonSerializer _jsonSerializer;

        public EditController(ILogger<EditController<T>> logger, ISql sql, IRomanizer romanizer, IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _sql = sql;
            _romanizer = romanizer;
            _jsonSerializer = jsonSerializer;
        }

        public virtual ActionResult Save(T model)
        {
            try
            {
                model.Name = BuildName(model);
                model.SerializeProperties();
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

        public virtual ActionResult Publish(T model)
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

        protected async Task<T> Get(Guid yid)
        {
            var model = await _sql.GetData<T>(yid);
            model.Roles = await _sql.GetRoles(yid);
            model.Users = await _sql.GetObjects<User>(yid, "user");
            model.Groups = await _sql.GetObjects<Group>(yid, "group");
            return model;
        }

        protected virtual string BuildName(T model)
        {
            return model.Name;
        }
    }
}
