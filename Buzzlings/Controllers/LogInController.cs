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

        public IActionResult LogIn()
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
            if (ModelState.IsValid)
            {
                User user = await _userService.GetByUsername(logInVM.Username!);
                if(user is null)
                {
                    ModelState.AddModelError("InvalidLoginAttempt", "Invalid login attempt.");
                    return View(logInVM);
                }

                bool passwordValid = await _userService.CheckPassword(user, logInVM.Password!);
                if (passwordValid == false)
                {
                    ModelState.AddModelError("InvalidLoginAttempt", "Invalid login attempt.");
                    return View(logInVM);
                }

                var result = await _signInManager.PasswordSignInAsync(user, logInVM.Password!, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return View(logInVM);
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
