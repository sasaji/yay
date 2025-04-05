using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class EmploymentStatusController(ILogger<EmploymentStatusController> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : EditController<EmploymentStatusViewModel>(logger, query, romanizer, jsonSerializer)
    {
        public async Task<IActionResult> Index(string id)
        {
            var model = !string.IsNullOrEmpty(id) ? await query.GetData<EmploymentStatusViewModel>(Guid.Parse(id)) : new EmploymentStatusViewModel { Id = Guid.NewGuid(), Type = "jobmode" };
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Policy(EmploymentStatus model)
        {
            return View("Index", model);
        }

        [HttpPost]
        public override IActionResult Save(EmploymentStatusViewModel model)
        {
            return base.Save(model);
        }
    }
}
