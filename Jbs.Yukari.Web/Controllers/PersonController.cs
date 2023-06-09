using Jbs.Yukari.Core;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Jbs.Yukari.Web.Controllers
{
    public class PersonController : EditController<PersonViewModel>
    {
        public PersonController(ILogger<PersonController> logger, ISql query, IRomanizer romanizer) : base(logger, query, romanizer) { }

        public async Task<ActionResult> Index(string yid)
        {
            var model = await Get(Guid.Parse(yid));
            model.RoleList = JsonConvert.SerializeObject(model.Roles, jsonSettings);
            model.OrganizationList = JsonConvert.SerializeObject(
                (await _sql.GetHierarchy("organization"))
                    .Select(x => new KeyValuePair<string, string>(x.Yid.ToString(), x.Text)), jsonSettings);
            model.TitleList = JsonConvert.SerializeObject(
                (await _sql.GetHierarchy("title"))
                    .Select(x => new KeyValuePair<string, string>(x.Yid.ToString(), x.Text)), jsonSettings);
            model.DeserializeProperties();
            return View("Index", model);
        }

        public IActionResult Translate(string[] names)
        {
            var romanSurname = string.Empty;
            var romanGivenName = string.Empty;
            var romanMiddleName = string.Empty;
            if (!string.IsNullOrWhiteSpace(names[0]) && !string.IsNullOrWhiteSpace(names[1]))
                romanSurname = _romanizer.Romanize(names[1], names[0]).Capitalize();
            if (!string.IsNullOrWhiteSpace(names[2]) && !string.IsNullOrWhiteSpace(names[3]))
                romanGivenName = _romanizer.Romanize(names[3], names[2]).Capitalize();
            if (!string.IsNullOrWhiteSpace(names[4]) && !string.IsNullOrWhiteSpace(names[5]))
                romanMiddleName = _romanizer.Romanize(names[5], names[4]).Capitalize();
            return Json(new { romanSurname, romanGivenName, romanMiddleName });
        }

        public IActionResult Policy(PersonViewModel model)
        {
            return View("Index", model);
        }

        public override ActionResult Save(PersonViewModel model)
        {
            model.Roles = JsonConvert.DeserializeObject<List<Dictionary<string, Role>>>(model.RoleList);
            return base.Save(model);
        }

        protected override string BuildName(PersonViewModel model)
        {
            return $"{model.Surname} {model.GivenName}";
        }
    }
}
