using System.Diagnostics;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services.Serialization;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IQuery query, IJsonSerializer jsonSerializer) : Controller
    {
        private readonly ILogger<HomeController> logger = logger;
        private readonly IQuery query = query;
        private readonly IJsonSerializer jsonSerializer = jsonSerializer;

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new HomeViewModel
            {
                TreeJson = $"[{jsonSerializer.Serialize(await query.GetOrganizationTree())}]"
            };
            return await Index(model);
        }

        /// <summary>
        /// ポストバック
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Index(HomeViewModel model)
        {
            var entities = await query.Search(model.SearchCriteria);
            model.TotalCount = entities.Count();
            model.SearchResult.Items = PaginatedList<BasicInfo>.Create(entities, model.FirstPage ? 1 : model.PageNumber, model.PageSize);
            model.FirstPage = false;
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}