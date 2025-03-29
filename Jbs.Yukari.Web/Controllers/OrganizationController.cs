using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class OrganizationController(ILogger<OrganizationController> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : EditController<Organization>(logger, query, romanizer, jsonSerializer)
    {
        public async Task<IActionResult> Index(string yid)
        {
            var model = !string.IsNullOrEmpty(yid) ? await query.GetData<OrganizationViewModel>(Guid.Parse(yid)) : new OrganizationViewModel { Yid = Guid.NewGuid(), Type = "organization" };
            return View("Index", model);
        }

        public IActionResult Policy(Organization model)
        {
            return View("Index", model);
        }

        public override IActionResult Save(Organization model)
        {
            return base.Save(model);
        }
    }
}
