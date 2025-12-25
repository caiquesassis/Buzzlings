using Buzzlings.BusinessLogic.Services.TopHive;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    public class TopHivesController : Controller
    {
        private readonly ITopHiveService _topHiveService;
        public TopHivesController(ITopHiveService topHiveService) 
        {
            _topHiveService = topHiveService;
        }

        public async Task<IActionResult> TopHives()
        {
            IEnumerable<Data.Models.TopHive>? topHives = await _topHiveService.GetTopHivesAsync();

            return View(topHives);
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
