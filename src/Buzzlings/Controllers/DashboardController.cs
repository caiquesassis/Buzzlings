using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.Simulation;
using Buzzlings.BusinessLogic.Services.TopHive;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Controllers;
using Buzzlings.Data.Constants;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
using Buzzlings.Web.Extensions;
using Buzzlings.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Buzzlings.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ISimulationService _simulationService;
        private readonly IUserService _userService;
        private readonly IHiveService _hiveService;
        private readonly IBuzzlingService _buzzlingService;
        private readonly ITopHiveService _topHiveService;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        private const string IgnoreHiveNameValidation = "IgnoreHiveNameValidation";
        private const string IgnoreBuzzlingNameValidation = "IgnoreBuzzlingNameValidation";
        private const string IsUpdateAttempt = "IsUpdateAttempt";

        public DashboardController(ISimulationService simulationService, IUserService userService,
            IHiveService hiveService, IBuzzlingService buzzlingService, ITopHiveService topHiveService,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _simulationService = simulationService;
            _userService = userService;
            _hiveService = hiveService;
            _buzzlingService = buzzlingService;
            _topHiveService = topHiveService;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(DashboardViewModel dashboardVM)
        {
            await PrepareDashboardViewModel(dashboardVM);
            return View(dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHive(DashboardViewModel dashboardVM)
        {
            TempData[IgnoreHiveNameValidation] = false;
            ModelState.Remove("BuzzlingName");
            await PrepareDashboardViewModel(dashboardVM);

            if (ModelState.IsValid is false)
            {
                return View("Index", dashboardVM);
            }

            Hive hive = new() { Name = dashboardVM.HiveName };

            await _hiveService.CreateHiveAsync(hive);

            dashboardVM.User!.Hive = hive;

            await _userService.UpdateUserAsync(dashboardVM.User);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBuzzling(DashboardViewModel dashboardVM)
        {
            TempData[IgnoreBuzzlingNameValidation] = false;
            ModelState.Remove("HiveName");
            await PrepareDashboardViewModel(dashboardVM);

            if (ModelState.IsValid is false)
            {
                return View("Index", dashboardVM);
            }

            await _hiveService.CreateBuzzlingAndAddToHiveAsync(dashboardVM.BuzzlingName!,
                int.Parse(dashboardVM.BuzzlingRole!), BuzzlingConstants.MoodDefaultValue,
                dashboardVM.User!.HiveId!.Value);

            TempData[IgnoreBuzzlingNameValidation] = true;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateBuzzling(int id)
        {
            Buzzling? buzzling = await _buzzlingService.GetBuzzlingByIdAsync(id);

            DashboardViewModel dashboardVM = new()
            {
                BuzzlingName = buzzling?.Name,
                BuzzlingRole = buzzling?.RoleId.ToString(),
                BuzzlingId = buzzling?.Id
            };

            await PrepareDashboardViewModel(dashboardVM);

            TempData[IsUpdateAttempt] = true;

            return View("Index", dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBuzzling(DashboardViewModel dashboardVM)
        {
            TempData[IgnoreBuzzlingNameValidation] = false;
            ModelState.Remove("HiveName");
            await PrepareDashboardViewModel(dashboardVM);

            if (ModelState.IsValid is false)
            {
                TempData[IsUpdateAttempt] = true;
                return View("Index", dashboardVM);
            }

            TempData[IgnoreBuzzlingNameValidation] = true;

            Buzzling? buzzling = await _buzzlingService.GetBuzzlingByIdAsync(dashboardVM.BuzzlingId!.Value);

            if (buzzling is null || buzzling.HiveId != dashboardVM.User!.HiveId)
            {
                return View("Index", dashboardVM);
            }

            buzzling.Name = dashboardVM.BuzzlingName;
            buzzling.RoleId = int.Parse(dashboardVM.BuzzlingRole!);

            await _buzzlingService.UpdateBuzzlingAsync(buzzling);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBuzzling(int id, DashboardViewModel dashboardVM)
        {
            await PrepareDashboardViewModel(dashboardVM);

            await _buzzlingService.DeleteBuzzlingByIdAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateHiveStatus(int lastLogIndex = 0, bool isInitialLoad = false)
        {
            //Ensure it's an AJAX request (Optional but helpful) so it doesn't trigger it live by other means
            if (Request.Headers["X-Requested-With"] != "XMLHttpRequest" && isInitialLoad is false)
            {
                return BadRequest();
            }

            int age;
            int happiness;

            if (isInitialLoad)
            {
                User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true);

                age = user?.Hive?.Age ?? 0;
                happiness = user?.Hive?.Happiness ?? 100;
            }
            else
            {
                age = await _simulationService.IncrementHiveAge(User.GetUserId());
                happiness = await _simulationService.ProcessSimulationAsync(User.GetUserId());
            }

            var (newLogs, updatedLogIndex) = await _simulationService.GetLatestEventLogs(User.GetUserId(), lastLogIndex);

            return Json(new { happiness, age, log = newLogs, lastLogIndex = updatedLogIndex });
        }

        public async Task<IActionResult> GetBuzzlingsTablePartial()
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true, true, true);

            if (user is null || user.Hive is null || user.Hive.Buzzlings is null)
            {
                return PartialView("_BuzzlingsTablePartial", new List<Buzzling>());
            }

            return PartialView("_BuzzlingsTablePartial", user.Hive.Buzzlings);
        }

        public async Task<IActionResult> FilterBuzzlings(string query)
        {
            List<Buzzling> buzzlings = new List<Buzzling>();

            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true, true, true);

            if (user is not null && user.Hive is not null && user.Hive.Buzzlings is not null)
            {
                buzzlings = user.Hive.Buzzlings.Where(b =>
                    string.IsNullOrEmpty(query) || //If the query is empty, return full list
                    string.IsNullOrWhiteSpace(query) ||
                    b.Name!.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return PartialView("_BuzzlingsTablePartial", buzzlings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizeHive()
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true);

            if (user?.Hive is null)
            {
                return BadRequest("User doesn't have a hive.");
            }

            await _topHiveService.AddTopHiveAsync(user.Id, user.Hive.Name!, user.Hive.Age!.Value);

            await _hiveService.DeleteHiveAsync(user.Hive);

            return Ok();
        }

        public async Task<IActionResult> LogOut()
        {
            IdentityResult result = await _userService.UpdateUserSecurityStampAsync(User.GetUserId());

            if (result.Succeeded is false)
            {
                ModelState.AddIdentityErrors(result.Errors, string.Empty);
                return View("Index");
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private async Task PrepareDashboardViewModel(DashboardViewModel dashboardVM)
        {
            // Set all possible cache-control headers to ensure no caching (so Username is correctly displayed)
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private, public, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";

            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true, true);

            if (user?.Hive?.Buzzlings is not null)
            {
                //This is needed to ensure they stay properly ordered in the table when updating
                user.Hive.Buzzlings = user.Hive.Buzzlings.OrderBy(b => b.Id).ToList();
            }

            dashboardVM.User = user;

            if (TempData[IgnoreHiveNameValidation] as bool? ?? true)
            {
                ModelState.Remove(nameof(dashboardVM.HiveName));
            }

            if (TempData[IgnoreBuzzlingNameValidation] as bool? ?? true)
            {
                ModelState.Remove(nameof(dashboardVM.BuzzlingName));
            }

            TempData[IsUpdateAttempt] = false;

            SelectList rolesSelectList = new SelectList(await _unitOfWork.BuzzlingRoleRepository.GetAllAsync(), "Id", "Name");

            ViewData["rolesSelectList"] = rolesSelectList;
        }
    }
}
