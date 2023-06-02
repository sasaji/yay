using System.Security.Cryptography;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class OrganizationController : EditController<Organization>
    {
        public OrganizationController(ILogger<OrganizationController> logger, IQuery query) : base(logger, query) { }

        public async Task<ActionResult> Index(string yid)
        {
            var model = await Get(yid);
            model.DeserializeProperties();
            return View(model);
        }

        public IActionResult Policy(Organization model)
        {
            return View("Index", model);
        }

        public override IActionResult Save(Organization model)
        {
            model.SerializeProperties();
            return base.Save(model);
        }
    }
}
