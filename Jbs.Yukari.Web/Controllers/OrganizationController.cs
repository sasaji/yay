using System.Security.Cryptography;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class OrganizationController : EditController<Organization>
    {
        public OrganizationController(ILogger<OrganizationController> logger, ISql query, IRomanizer romanizer) : base(logger, query, romanizer) { }

        public async Task<ActionResult> Index(string yid)
        {
            var model = await Get(Guid.Parse(yid));
            model.DeserializeProperties();
            return View("Index", model);
        }

        public IActionResult Policy(Organization model)
        {
            return View("Index", model);
        }

        public override ActionResult Save(Organization model)
        {
            return base.Save(model);
        }
    }
}
