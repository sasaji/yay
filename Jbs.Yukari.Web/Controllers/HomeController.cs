using System.Diagnostics;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;
using Jbs.Yukari.Core.Services;
using Jbs.Yukari.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jbs.Yukari.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISql _query;
        private readonly IJsonSerializer _jsonSerializer;

        public HomeController(ILogger<HomeController> logger, ISql query, IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _query = query;
            _jsonSerializer = jsonSerializer;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new HomeViewModel
            {
                TreeJson = $"[{_jsonSerializer.Serialize(await _query.GetTree("organization"))}]"
            };
            return View(model);
        }

        /// <summary>
        /// ポストバック
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Index(HomeViewModel model)
        {
            var entities = await _query.Search(model.SearchCriteria);
            model.TotalCount = entities.Count();
            model.SearchResult.Items = PaginatedList<BasicInfoOutline>.Create(entities, model.FirstPage ? 1 : model.PageNumber, model.PageSize);
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