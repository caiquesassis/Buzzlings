using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Buzzlings.Web.Extensions;
using Buzzlings.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHiveService _hiveService;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IUserService userService, IHiveService hiveService,
            SignInManager<User> signInManager)
        {
            _userService = userService;
            _hiveService = hiveService;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
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
            if (ModelState.IsValid is false)
            {
                return View(updateUsernameVM);
            }

            (IdentityResult result, User? user) = await _userService.ChangeUserNameAsync(User.GetUserId(), updateUsernameVM.Username!);

            if (result.Succeeded is false)
            {
                ModelState.AddIdentityErrors(result, "InvalidRegistrationAttempt");

                return View(updateUsernameVM);
            }

            //Re-sign in the user to update claims in the authentication cookie
            //This is so the username change is reflected...
            await _signInManager.RefreshSignInAsync(user!);

            return RedirectToAction("UpdateSuccess", "Account");
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
            if (ModelState.IsValid is false)
            {
                return View(updatePasswordVM);
            }

            (IdentityResult result, User? user) = await _userService.UpdateUserPasswordAsync(User.GetUserId(), updatePasswordVM.CurrentPassword!, updatePasswordVM.NewPassword!);

            if (result.Succeeded is false)
            {
                ModelState.AddIdentityErrors(result, "InvalidRegistrationAttempt");

                return View(updatePasswordVM);
            }

            // Update the security stamp to invalidate existing sessions
            //ACTUALLY, when you change password, this already gets called automatically (supposedly)
            //await _userService.UpdateSecurityStampAsync(user!);

            //Re-sign in the user to update claims in the authentication cookie
            await _signInManager.RefreshSignInAsync(user!);

            return RedirectToAction("UpdateSuccess", "Account");
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
            // Update the security stamp to invalidate existing sessions
            IdentityResult result = await _userService.UpdateUserSecurityStampAsync(User.GetUserId());

            if (result.Succeeded is false)
            {
                ModelState.AddIdentityErrors(result.Errors, string.Empty);
                return View();
            }

            await _signInManager.SignOutAsync();

            result = await _userService.DeleteUserAsync(User.GetUserId());

            if (result.Succeeded is false)
            {
                ModelState.AddIdentityErrors(result.Errors, string.Empty);
                return View();
            }

            return RedirectToAction("DeleteAccountSuccess", "Account");
        }

        [Authorize]
        public async Task<IActionResult> ChangeHiveName()
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId());

            UpdateHiveNameViewModel updateHiveNameVM = new UpdateHiveNameViewModel { User = user };

            return View(updateHiveNameVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeHiveName(UpdateHiveNameViewModel updateHiveNameVM)
        {
            if (ModelState.IsValid is false)
            {
                return View(updateHiveNameVM);
            }

            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Couldn't find user.");
                return View(updateHiveNameVM);
            }

            updateHiveNameVM.User = user;

            if (user.Hive is null)
            {
                ModelState.AddModelError(string.Empty, "User doesn't have a hive.");
                return View(updateHiveNameVM);
            }

            await _hiveService.UpdateHiveNameAsync(user.Hive, updateHiveNameVM.HiveName!);

            return RedirectToAction("UpdateSuccess", "Account");
        }

        [Authorize]
        public async Task<IActionResult> DeleteHive()
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId());

            return View(user);
        }

        [Authorize]
        [HttpPost, ActionName("DeleteHive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHiveConfirmed()
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View();
            }

            if (user.Hive is null)
            {
                ModelState.AddModelError(string.Empty, "User doesn't have a hive.");
                return View();
            }

            await _hiveService.DeleteHiveAsync(user.Hive);

            return RedirectToAction("DeleteHiveSuccess", "Account");
        }

        [Authorize]
        public IActionResult UpdateSuccess()
        {
            return View();
        }

        public IActionResult DeleteAccountSuccess()
        {
            return View();
        }

        [Authorize]
        public IActionResult DeleteHiveSuccess()
        {
            return View();
        }
    }
}
