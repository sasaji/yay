using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Jbs.Yukari.Web.Models;
using System.Xml.Linq;
using System.Linq;

namespace Jbs.Yukari.Web.Controllers
{
    public class PersonController : EditController<Person>
    {
        public PersonController(ILogger<PersonController> logger, IQuery query) : base(logger, query) { }

        public async Task<ActionResult> Index(string yid)
        {
            var model = await Get(yid);
            model.DeserializeProperties();
            return View(model);
        }

        public IActionResult Translate(Person model)
        {
            ModelState.Clear();
            model.RomanSurname = "henkan";
            return View("Index", model);
        }

        public IActionResult Policy(Person model)
        {
            return View("Index", model);
        }

        public override IActionResult Save(Person model)
        {
            model.SerializeProperties();
            return base.Save(model);
        }
    }
}
