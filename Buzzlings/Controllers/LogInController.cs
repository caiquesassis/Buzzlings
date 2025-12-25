using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Buzzlings.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    public class LogInController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public LogInController(IUserService userService, SignInManager<User> signInManager) 
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel logInVM)
        {
            if (ModelState.IsValid is false)
            {
                return View("Index", logInVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(logInVM.Username!, logInVM.Password!, false, false);

            if (result.Succeeded is false)
            {
                // Generic error message for security (prevents username enumeration)
                ModelState.AddModelError("InvalidLoginAttempt", "Invalid login attempt.");

                return View("Index", logInVM);
            }

            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }
    }
}
