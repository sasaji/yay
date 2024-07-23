using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public abstract class EditController<T> : Controller where T : BasicInfo
    {
        protected readonly ILogger<EditController<T>> logger;
        protected readonly IQuery query;
        protected readonly IRomanizer romanizer;
        protected readonly IJsonSerializer jsonSerializer;

        public EditController(ILogger<EditController<T>> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer)
        {
            this.logger = logger;
            this.query = query;
            this.romanizer = romanizer;
            this.jsonSerializer = jsonSerializer;
        }

        public virtual IActionResult Save(T model)
        {
            try
            {
                model.Name = BuildName(model);
                model.SerializeProperties();
                query.Save(model);
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
                query.Publish(model.Yid);
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
            var model = await query.GetData<T>(yid);
            model.Roles = await query.GetRoles(yid);
            model.Enrollment = await query.GetEnrollment(yid);
            model.Users = await query.GetObjects<User>(yid, "user");
            model.Groups = await query.GetObjects<Group>(yid, "group");
            return model;
        }

        protected virtual string BuildName(T model)
        {
            return model.Name;
        }
    }
}
