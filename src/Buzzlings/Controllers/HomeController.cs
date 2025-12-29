using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
