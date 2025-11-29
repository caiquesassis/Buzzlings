using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using Buzzlings.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHiveService _hiveService;
        private readonly SignInManager<User> _signInManager;

        public DashboardController(IUserService userService, IHiveService hiveService, SignInManager<User> signInManager)
        {
            _userService = userService;
            _hiveService = hiveService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(DashboardViewModel dashboardVM)
        {
            // Set all possible cache-control headers to ensure no caching (so Username is correctly displayed)
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private, public, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";

            User user = await _userService.GetUser(User);

            dashboardVM.User = user;

            if (dashboardVM.User.HiveId.HasValue)
            {
                dashboardVM.User.Hive = await _hiveService.GetById(dashboardVM.User.HiveId.Value);
            }

            if (dashboardVM.IgnoreHiveNameValidation)
            {
                if (ModelState.ContainsKey("HiveName"))
                {
                    ModelState["HiveName"]?.Errors.Clear();
                }
            }

            if (dashboardVM.IgnoreBuzzlingNameValidation)
            {
                if (ModelState.ContainsKey("BuzzlingName"))
                {
                    ModelState["BuzzlingName"]?.Errors.Clear();
                }
            }

            return View(dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHive(DashboardViewModel dashboardVM)
        {
            dashboardVM.IgnoreHiveNameValidation = false;
            dashboardVM.IgnoreBuzzlingNameValidation = true;

            ModelState.Remove("BuzzlingName");

            if (ModelState.IsValid)
            {
                Hive hive = new Hive { Name = dashboardVM.HiveName };

                await _hiveService.Create(hive);

                dashboardVM.User = await _userService.GetUser(User);

                dashboardVM.User.Hive = hive;

                await _userService.Update(dashboardVM.User);
            }

            return RedirectToAction("Index", "Dashboard", dashboardVM);
        }

        public async Task UpdateSimulationState()
        {

        }

        public async Task<IActionResult> GetEventLogs()
        {
            User user = await _userService.GetUser(User);

            if (user.HiveId.HasValue)
            {
                Hive hive = await _hiveService.GetById(user.HiveId.Value);

                if (hive.EventLog is null)
                {
                    hive.EventLog = new List<string>();

                    hive.EventLog?.Add(hive.Name + " is born!");

                    await _hiveService.Update(hive);
                }

                return Json(new { logs = hive.EventLog });
            }

            return Json(new { logs = new List<string>() });
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
