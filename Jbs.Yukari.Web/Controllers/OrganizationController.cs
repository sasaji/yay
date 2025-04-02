using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class OrganizationController(ILogger<OrganizationController> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : EditController<OrganizationViewModel>(logger, query, romanizer, jsonSerializer)
    {
        public async Task<IActionResult> Index(string id)
        {
            var model = !string.IsNullOrEmpty(id) ? await query.GetData<OrganizationViewModel>(Guid.Parse(id)) : new OrganizationViewModel { Id = Guid.NewGuid(), Type = "organization" };
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Policy(Organization model)
        {
            return View("Index", model);
        }

        [HttpPost]
        public override IActionResult Save(OrganizationViewModel model)
        {
            return base.Save(model);
        }
    }
}
