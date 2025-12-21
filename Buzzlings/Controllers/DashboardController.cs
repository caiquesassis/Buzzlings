using Buzzlings.BusinessLogic.Dtos;
using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.BusinessLogic.Simulation;
using Buzzlings.BusinessLogic.Utils;
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
        private readonly IUserService _userService;
        private readonly IHiveService _hiveService;
        private readonly IBuzzlingService _buzzlingService;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        private readonly SimulationEventHandler _simulationEventHandler;

        private const string IgnoreHiveNameValidation = "IgnoreHiveNameValidation";
        private const string IgnoreBuzzlingNameValidation = "IgnoreBuzzlingNameValidation";
        private const string IsUpdateAttempt = "IsUpdateAttempt";

        public DashboardController(IUserService userService,
            IHiveService hiveService, IBuzzlingService buzzlingService,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _hiveService = hiveService;
            _buzzlingService = buzzlingService;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;

            _simulationEventHandler = new SimulationEventHandler();
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

            return RedirectToAction("Index", "Dashboard");
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

            return RedirectToAction("Index", "Dashboard");
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

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBuzzling(DashboardViewModel dashboardVM, int id)
        {
            await PrepareDashboardViewModel(dashboardVM);

            await _buzzlingService.DeleteBuzzlingByIdAsync(id);

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> UpdateHiveHappiness()
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true, true, true);

            if (user is null || user.Hive is null)
            {
                return Json(new { happiness = 0 });
            }

            SimulationEventDto simulationEvent =
                _simulationEventHandler.GenerateEvent(
                    user.Hive.Buzzlings is not null ? user.Hive.Buzzlings.ToList() : new List<Buzzling>(),
                    user.Hive.EventLog?.Last());

            if (user.Hive.Buzzlings is not null)
            {
                if (simulationEvent.buzzlingsToDelete > 0)
                {
                    List<Buzzling> buzzlingsToDelete = new List<Buzzling>();

                    for (int i = 0; i < simulationEvent.buzzlingsToDelete; i++)
                    {
                        Buzzling b = user.Hive.Buzzlings.ElementAt(RandomUtils.GetRandomRangeValue(0, user.Hive.Buzzlings.Count - 1));

                        buzzlingsToDelete.Add(b);

                        user.Hive.Buzzlings.Remove(b);
                    }

                    // This prevents the SaveAsync inside DeleteBuzzlingsRangeAsync from crashing
                    _unitOfWork.DetachRange(buzzlingsToDelete);

                    await _buzzlingService.DeleteBuzzlingsRangeAsync(buzzlingsToDelete);

                    await _hiveService.UpdateHiveAsync(user.Hive);
                }
            }

            user.Hive.Happiness += simulationEvent.happinessImpact;
            //hive.Happiness -= 50;
            user.Hive.Happiness = Math.Clamp(user.Hive.Happiness!.Value, 0, 100);

            user.Hive.EventLog?.Add(simulationEvent.log);

            await _hiveService.UpdateHiveAsync(user.Hive);

            Console.WriteLine("HAPPINESS UPDATED.");

            return Json(new { happiness = user.Hive.Happiness });
        }

        public async Task<IActionResult> UpdateHiveAge()
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true);

            if (user is null || user.Hive is null)
            {
                return Json(new { age = 0 });
            }

            if (user.Hive!.Happiness > 0)
            {
                user.Hive.Age++;
            }

            await _hiveService.UpdateHiveAsync(user.Hive);

            return Json(new { age = user.Hive.Age });
        }

        public async Task<IActionResult> GetEventLog(int lastLogIndex = 0)
        {
            User? user = await _userService.GetUserByIdAsync(User.GetUserId(), true);

            if (user is null || user.Hive is null)
            {
                return Json(new { log = new List<string>() });
            }

            if (user.Hive.EventLog is null)
            {
                user.Hive.EventLog = ["🍯 " + user.Hive.Name + " 🍯 is born!"];

                await _hiveService.UpdateHiveAsync(user.Hive);
            }

            List<string> newLogs = user.Hive.EventLog.Skip(lastLogIndex).ToList();

            int updatedLogIndex = user.Hive.EventLog.Count;

            Console.WriteLine("LOGS FETCHED.");

            return Json(new { log = newLogs, lastLogIndex = updatedLogIndex });
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

        public void DeleteHiveOnSimulationEnd()
        {

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

            return RedirectToAction("Index", "Home");
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
