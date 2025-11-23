using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
