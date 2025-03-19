using Jbs.Yukari.Core;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Romanization;
using Jbs.Yukari.Core.Services.Serialization;
using Jbs.Yukari.Domain;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class PersonController : EditController<PersonViewModel>
    {
        private readonly IPersonQuery query;
        private readonly IRomanizer romanizer;
        private readonly IJsonSerializer jsonSerializer;

        public PersonController(IPersonQuery query, IRomanizer romanizer, IJsonSerializer jsonSerializer) : base(query)
        {
            this.query = query;
            this.romanizer = romanizer;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task<IActionResult> Index(string yid)
        {
            var model = !string.IsNullOrEmpty(yid) ? await query.GetPerson<PersonViewModel>(Guid.Parse(yid)) : new PersonViewModel();

            model.RolesViewModel = jsonSerializer.Serialize(model.Roles);
            model.TreeJson = $"[{jsonSerializer.Serialize(await query.GetTree("organization"))}]";
            model.TitlesJson = jsonSerializer.Serialize(
                (await query.GetHierarchy("title"))
                    .Select(x => new KeyValuePair<string, string>(x.Yid.ToString(), x.Text)));
            model.Enrollments = [.. (await query.GetEnrollments())];
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
            User u = (User)x[0];
            return View("Index", model);
        }

        [HttpPost]
        public override IActionResult Save(PersonViewModel model)
        {
            model.Roles = jsonSerializer.Deserialize<List<Dictionary<string, Relation>>>(model.RolesViewModel);
            return base.Save(model);
        }

        protected override string BuildName(PersonViewModel model)
        {
            return $"{model.Surname} {model.GivenName}".Trim();
        }
    }
}
