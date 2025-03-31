using Jbs.Yukari.Core;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Jbs.Yukari.Domain;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jbs.Yukari.Web.Controllers
{
    public class PersonController(ILogger<PersonController> logger, IQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : EditController<PersonViewModel>(logger,query, romanizer, jsonSerializer)
    {
        public async Task<IActionResult> Index(string yid)
        {
            var model = !string.IsNullOrEmpty(yid) ? await query.GetData<PersonViewModel>(Guid.Parse(yid)) : new PersonViewModel { Yid = Guid.NewGuid(), Type = "person" };
            model.RolesViewModel = jsonSerializer.Serialize(model.Roles);
            model.TreeJson = $"[{jsonSerializer.Serialize(await query.GetOrganizationTree())}]";
            model.Titles = [.. (await query.GetList("title", false)).Select(x => new SelectListItem(x.Name, x.Yid.ToString()))];
            model.EmploymentStatuses = [.. (await query.GetList("jobmode", true)).Select(x => new SelectListItem(x.Name, x.Yid.ToString()))];
            return View("Index", model);
        }

        public IActionResult Romanize(string[] names)
        {
            var romanSurname = string.Empty;
            var romanGivenName = string.Empty;
            var romanMiddleName = string.Empty;
            if (!string.IsNullOrWhiteSpace(names[0]) && !string.IsNullOrWhiteSpace(names[1]))
                romanSurname = romanizer.Romanize(names[1], names[0]).Capitalize();
            if (!string.IsNullOrWhiteSpace(names[2]) && !string.IsNullOrWhiteSpace(names[3]))
                romanGivenName = romanizer.Romanize(names[3], names[2]).Capitalize();
            if (!string.IsNullOrWhiteSpace(names[4]) && !string.IsNullOrWhiteSpace(names[5]))
                romanMiddleName = romanizer.Romanize(names[5], names[4]).Capitalize();
            return Json(new { romanSurname, romanGivenName, romanMiddleName });
        }

        [HttpPost]
        public IActionResult Policy(PersonViewModel model)
        {
            var trans = new PersonTransformer();
            var x = trans.Transform(model);
            //User u = (User)x[0];
            return View("Index", model);
        }

        [HttpPost]
        public override IActionResult Save(PersonViewModel model)
        {
            model.Roles = jsonSerializer.Deserialize<List<Dictionary<string, BasicInfo>>>(model.RolesViewModel);
            return base.Save(model);
        }
    }
}
