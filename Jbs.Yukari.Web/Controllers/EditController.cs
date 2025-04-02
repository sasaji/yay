using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public abstract class EditController<T>(ILogger<EditController<T>> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : Controller where T : BasicInfo
    {
        protected readonly ILogger<EditController<T>> logger = logger;
        protected readonly IQuery query = query;
        protected readonly IRomanizer romanizer = romanizer;
        protected readonly IJsonSerializer jsonSerializer = jsonSerializer;

        public virtual IActionResult Save(T model)
        {
            DoSave(model, 1);
            ViewData["Action"] = "一時保存";
            return View("Index", model);
        }

        public virtual IActionResult CheckIn(T model)
        {
            DoSave(model, 2);
            ViewData["Action"] = "チェックイン";
            return View("Index", model);
        }

        public virtual IActionResult Publish(T model)
        {
            DoPublish(model, false);
            ViewData["Action"] = "反映";
            return View("Index", model);
        }

        public virtual IActionResult PublishData(T model)
        {
            DoPublish(model, true);
            ViewData["Action"] = "データのみ反映";
            return View("Index", model);
        }

        protected virtual string BuildName(T model)
        {
            return model.Name;
        }

        public virtual IActionResult CheckOut(T model)
        {
            DoSave(model, 1);
            ViewData["Action"] = "チェックアウト";
            return View("Index", model);
        }

        private void DoSave(T model, int phase)
        {
            try
            {
                model.Name = BuildName(model);
                model.SerializeProperties();
                model.Phase = phase;
                query.Save(model);
                ViewData["Result"] = "0";
            }
            catch (Exception ex)
            {
                ViewData["Result"] = "1";
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        private void DoPublish(T model, bool dbOnly)
        {
            try
            {
                query.Publish(model.Id);
                model.Phase = 0;
                ViewData["Result"] = "0";
            }
            catch (Exception ex)
            {
                ViewData["Result"] = "1";
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
