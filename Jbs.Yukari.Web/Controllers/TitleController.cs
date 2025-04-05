using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class TitleController(ILogger<TitleController> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : EditController<TitleViewModel>(logger, query, romanizer, jsonSerializer)
    {
        public async Task<IActionResult> Index(string id)
        {
            var model = !string.IsNullOrEmpty(id) ? await query.GetData<TitleViewModel>(Guid.Parse(id)) : new TitleViewModel { Id = Guid.NewGuid(), Type = "title" };
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Policy(Title model)
        {
            return View("Index", model);
        }

        [HttpPost]
        public override IActionResult Save(TitleViewModel model)
        {
            return base.Save(model);
        }
    }
}
