using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
