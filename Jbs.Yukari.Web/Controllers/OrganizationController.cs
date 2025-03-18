using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class OrganizationController : EditController<Organization>
    {
        public OrganizationController(ILogger<OrganizationController> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : base(logger, query, romanizer, jsonSerializer) { }

        public async Task<IActionResult> Index(string yid)
        {
            var model = await Get(Guid.Parse(yid));
            model.DeserializeProperties();
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
