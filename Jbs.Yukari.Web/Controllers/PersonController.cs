using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Jbs.Yukari.Web.Models;
using System.Xml.Linq;
using System.Linq;
using Jbs.Yukari.Core;
using Jbs.Yukari.Core.Services;
using System.Reflection;

namespace Jbs.Yukari.Web.Controllers
{
    public class PersonController : EditController<PersonViewModel>
    {
        public PersonController(ILogger<PersonController> logger, ISql query, IRomanizer romanizer) : base(logger, query, romanizer) { }

        public async Task<ActionResult> Index(string yid)
        {
            var model = await Get(yid);
            model.DeserializeProperties();
            return View(model);
        }

        public IActionResult Translate(Person model)
        {
            ModelState.Clear(); // これがないとローマ字氏名を上書きできない。
            if (!string.IsNullOrWhiteSpace(model.KanaSurname) && !string.IsNullOrWhiteSpace(model.Surname))
                model.RomanSurname = _romanizer.Romanize(model.KanaSurname, model.Surname).Capitalize();
            if (!string.IsNullOrWhiteSpace(model.KanaGivenName) && !string.IsNullOrWhiteSpace(model.GivenName))
                model.RomanGivenName = _romanizer.Romanize(model.KanaGivenName, model.GivenName).Capitalize();
            return View("Index", model);
        }

        public IActionResult Policy(Person model)
        {
            return View("Index", model);
        }

        public override IActionResult Save(PersonViewModel model)
        {
            model.SerializeProperties();
            return base.Save(model);
        }

        protected override string BuildName(PersonViewModel model)
        {
            return $"{model.Surname} {model.GivenName}";
        }
    }
}
