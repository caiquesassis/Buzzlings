using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Buzzlings.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IUserService userService, SignInManager<User> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult ChangeUsername()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUsername(UpdateUsernameViewModel updateUsernameVM)
        {
            if (string.IsNullOrWhiteSpace(updateUsernameVM.Username) == false)
            {
                //Check if there's someone already using the provided username
                if (await _userService.GetByUsername(updateUsernameVM.Username) is not null)
                {
                    ModelState.AddModelError("Username", "This username is already in use.");

                    //I need to pass an extra message that there's already someone with that username...
                    return View(updateUsernameVM);
                }
            }

            if (ModelState.IsValid)
            {
                User user = await _userService.GetUser(User);

                if(user is not null)
                {
                    user.UserName = updateUsernameVM.Username;

                    IdentityResult result = await _userService.Update(user);

                    if (result.Succeeded)
                    {
                        //Re-sign in the user to update claims in the authentication cookie
                        //This is so the username change is reflected...
                        await _signInManager.RefreshSignInAsync(user);

                        return RedirectToAction("UpdateSuccess", "Account");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("InvalidRegistrationAttempt", error.Description);
                        }
                    }
                }
                else
                {
                    throw new NullReferenceException("Couldn't find user.");
                }
            }

            return View(updateUsernameVM);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UpdatePasswordViewModel updatePasswordVM)
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.GetUser(User);

                if (user is not null)
                {
                    IdentityResult result = await _userService.UpdatePassword(user, updatePasswordVM.CurrentPassword!, updatePasswordVM.NewPassword!);

                    if (result.Succeeded)
                    {
                        // Update the security stamp to invalidate existing sessions
                        await _userService.UpdateSecurityStamp(user);

                        //Re-sign in the user to update claims in the authentication cookie
                        await _signInManager.RefreshSignInAsync(user);

                        return RedirectToAction("UpdateSuccess", "Account");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("InvalidRegistrationAttempt", error.Description);
                        }
                    }
                }
                else
                {
                    throw new NullReferenceException("Couldn't find user.");
                }
            }

            return View(updatePasswordVM);
        }

        [Authorize]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [Authorize]
        [HttpPost, ActionName("DeleteAccount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            User user = await _userService.GetUser(User);

            if (user is not null)
            {
                var result = await _userService.Delete(user);

                if (result.Succeeded)
                {
                    // Update the security stamp to invalidate existing sessions
                    await _userService.UpdateSecurityStamp(user);

                    await _signInManager.SignOutAsync();

                    return RedirectToAction("DeleteSuccess", "Account");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("InvalidRegistrationAttempt", error.Description);
                    }
                }
            }
            else
            {
                throw new NullReferenceException("Couldn't find user.");
            }

            return View();
        }

        [Authorize]
        public IActionResult UpdateSuccess()
        {
            return View();
        }

        public IActionResult DeleteSuccess()
        {
            return View();
        }

        [Authorize]
        public IActionResult Back()
        {
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
