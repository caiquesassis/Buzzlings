using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public DashboardController(IUnitOfWork unitOfWork, IUserService userService, SignInManager<User> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            // Set all possible cache-control headers to ensure no caching (so Username is correctly displayed)
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private, public, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            User user = await _userService.GetUser(User);

            if (user is not null)
            {
                // Update the security stamp to invalidate existing sessions
                await _userService.UpdateSecurityStamp(user);
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
