using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
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
            if (string.IsNullOrWhiteSpace(registerVM.Username) == false)
            {
                //Check if there's someone already using the provided username
                if (await _userService.GetByUsername(registerVM.Username) is not null)
                {
                    ModelState.AddModelError("Username", "This username is already in use.");

                    //I need to pass an extra message that there's already someone with that username...
                    return View(registerVM);
                }
            }

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = registerVM.Username
                };

                IdentityResult result = await _userService.Create(user, registerVM.Password!);

                if (result.Succeeded)
                {
                    return RedirectToAction("RegisterSuccess", "Register");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("InvalidRegistrationAttempt", error.Description);
                    }
                }
            }

            return View(registerVM);
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
