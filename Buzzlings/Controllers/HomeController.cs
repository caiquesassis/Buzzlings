using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return RedirectToAction("Index", "LogIn");
        }

        public IActionResult Register()
        {
            return RedirectToAction("Index", "Register");
        }

        public IActionResult TopHives()
        {
            return RedirectToAction("Index", "TopHives");
        }

        public IActionResult About()
        {
            return RedirectToAction("Index", "About");
        }
    }
}
