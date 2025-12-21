using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Buzzlings.Web.Extensions;
using Buzzlings.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService) 
        {
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid is false)
            {
                return View(registerVM);
            }

            IdentityResult result = await _userService.RegisterNewUserAsync(registerVM.Username!, registerVM.Password!);

            if (result.Succeeded is false)
            {
                ModelState.AddIdentityErrors(result.Errors.Where(e => e.Code is "DuplicateUsername"), "Username");
                ModelState.AddIdentityErrors(result.Errors.Where(e => e.Code is not "DuplicateUsername"), "InvalidRegistrationAttempt");

                return View(registerVM);
            }

            return RedirectToAction("RegisterSuccess", "Register");
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
