using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public abstract class EditController<T> : Controller where T : BasicInfo
    {
        protected readonly ILogger<EditController<T>> _logger;
        protected readonly IQuery _query;
        protected readonly IRomanizer _romanizer;
        protected readonly IJsonSerializer _jsonSerializer;

        public EditController(ILogger<EditController<T>> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _query = query;
            _romanizer = romanizer;
            _jsonSerializer = jsonSerializer;
        }

        public virtual ActionResult Save(T model)
        {
            try
            {
                model.Name = BuildName(model);
                model.SerializeProperties();
                _query.Save(model);
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
                _query.Publish(model.Yid);
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
            var model = await _query.GetData<T>(yid);
            model.Roles = await _query.GetRoles(yid);
            model.Enrollment = await _query.GetEnrollment(yid);
            model.Users = await _query.GetObjects<User>(yid, "user");
            model.Groups = await _query.GetObjects<Group>(yid, "group");
            return model;
        }

        protected virtual string BuildName(T model)
        {
            return model.Name;
        }
    }
}
