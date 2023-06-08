using System.Data;
using System.Reflection;
using Jbs.Yukari.Core;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static Dapper.SqlMapper;

namespace Jbs.Yukari.Web.Controllers
{
    public class PersonController : EditController<PersonViewModel>
    {
        public PersonController(ILogger<PersonController> logger, ISql query, IRomanizer romanizer) : base(logger, query, romanizer) { }

        public async Task<ActionResult> Index(string yid)
        {
            var model = await Get(yid);
            model.RoleList = JsonConvert.SerializeObject(model.Roles, jsonSettings);
            model.DeserializeProperties();
            return View(model);
        }

        public IActionResult Translate(PersonViewModel model)
        {
            ModelState.Clear(); // これがないとローマ字氏名を上書きできない。
            if (!string.IsNullOrWhiteSpace(model.KanaSurname) && !string.IsNullOrWhiteSpace(model.Surname))
                model.RomanSurname = _romanizer.Romanize(model.KanaSurname, model.Surname).Capitalize();
            if (!string.IsNullOrWhiteSpace(model.KanaGivenName) && !string.IsNullOrWhiteSpace(model.GivenName))
                model.RomanGivenName = _romanizer.Romanize(model.KanaGivenName, model.GivenName).Capitalize();
            return View("Index", model);
        }

        public IActionResult Policy(PersonViewModel model)
        {
            return View("Index", model);
        }

        public override IActionResult Save(PersonViewModel model)
        {
            model.SerializeProperties();
            return base.Save(model);
        }

        public IActionResult AddRole(PersonViewModel model)
        {
            return View("Index", model);
        }

        protected override string BuildName(PersonViewModel model)
        {
            return $"{model.Surname} {model.GivenName}";
        }

        private List<SelectListItem> BuildRoleList(List<KeyValuePair<int, Dictionary<string, Role>>> roles)
        {
            return roles.Select((x, i) => new SelectListItem
            {
                Text = $"{x.Value["organization"].Name} / {x.Value["title"].Name}".Trim(),
                Value = $"{JsonConvert.SerializeObject(x, jsonSettings)}"
            }).ToList();
        }
    }
}
