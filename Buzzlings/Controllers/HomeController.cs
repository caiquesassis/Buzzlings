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
            return RedirectToAction("LogIn", "LogIn");
        }

        public IActionResult Register()
        {
            return RedirectToAction("Register", "Register");
        }

        public IActionResult TopHives()
        {
            return View();
        }

        public IActionResult About()
        {
            return RedirectToAction("About", "About");
        }
    }
}
