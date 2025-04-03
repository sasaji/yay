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
        public async Task<IActionResult> Index(string id)
        {
            var model = !string.IsNullOrEmpty(id) ? await query.GetData<PersonViewModel>(Guid.Parse(id)) : new PersonViewModel { Id = Guid.NewGuid(), Type = "person" };
            model.AffiliationsViewModel = [.. model.Affiliations.Select(x => new SelectListItem(x.Organization.Name + "/" + x.Title.Name, x.Organization.Id + "/" + x.Title.Id))];
            model.TreeJson = $"[{jsonSerializer.Serialize(await query.GetOrganizationTree(string.Empty))}]";
            model.Titles = [.. (await query.GetIdNamePairs("title", false)).Select(x => new SelectListItem(x.Name, x.Id.ToString()))];
            model.EmploymentStatuses = [.. (await query.GetIdNamePairs("jobmode", true)).Select(x => new SelectListItem(x.Name, x.Id.ToString()))];
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
            //model.Roles = jsonSerializer.Deserialize<List<Dictionary<string, IdNamePair>>>(model.RolesViewModel);
            model.Affiliations = model.AffiliationsViewModel
                .Select(x => new Affiliation
                {
                    Organization = new IdNamePair
                    {
                        Id = Guid.Parse(x.Value.Split('/')[0]),
                        Name = x.Text.Split('/')[0]
                    },
                    Title = new IdNamePair
                    {
                        Id = Guid.Parse(x.Value.Split('/')[1]),
                        Name = x.Text.Split('/')[1]
                    }
                });
            return base.Save(model);
        }
    }
}
