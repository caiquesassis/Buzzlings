using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Buzzlings.Data.Repositories.Interfaces;
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

        public DashboardController(IUserService userService,
            IHiveService hiveService, IBuzzlingService buzzlingService,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _hiveService = hiveService;
            _buzzlingService = buzzlingService;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
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
                dashboardVM.User.Hive = await _hiveService.Get(h => h.Id == dashboardVM.User.HiveId, "Buzzlings");
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

            SelectList rolesSelectList = new SelectList(await _unitOfWork.BuzzlingRoleRepository.GetAll(), "Id", "Name");

            ViewData["rolesSelectList"] = rolesSelectList;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBuzzling(DashboardViewModel dashboardVM)
        {
            dashboardVM.IgnoreHiveNameValidation = true;
            dashboardVM.IgnoreBuzzlingNameValidation = false;

            ModelState.Remove("HiveName");

            if (ModelState.IsValid)
            {
                Buzzling buzzling = new Buzzling
                {
                    Name = dashboardVM.BuzzlingName,
                    Role = await _unitOfWork.BuzzlingRoleRepository.Get((r) => r.Id == Int32.Parse(dashboardVM.BuzzlingRole!)),
                    Mood = 100
                };

                await _buzzlingService.Create(buzzling);

                dashboardVM.User = await _userService.GetUser(User);

                dashboardVM.User.Hive = await _hiveService.Get(h => h.Id == dashboardVM.User.HiveId, "Buzzlings");

                dashboardVM.User.Hive?.Buzzlings?.Add(buzzling);

                await _hiveService.Update(dashboardVM.User?.Hive!);

                await _userService.Update(dashboardVM.User!);

                dashboardVM.BuzzlingName = String.Empty;
                dashboardVM.BuzzlingRole = "0";

                dashboardVM.IgnoreBuzzlingNameValidation = true;
            }

            return RedirectToAction("Index", "Dashboard", dashboardVM);
        }


        public async Task<IActionResult> UpdateBuzzling(int id)
        {
            Buzzling buzzling = await _buzzlingService.GetById(id);

            DashboardViewModel dashboardVM = new DashboardViewModel
            {
                BuzzlingName = buzzling.Name,
                BuzzlingRole = buzzling.RoleId.ToString(),
                BuzzlingId = buzzling.Id,
                IsUpdateAttempt = true
            };

            return RedirectToAction("Index", "Dashboard", dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBuzzling(DashboardViewModel dashboardVM)
        {
            dashboardVM.IgnoreHiveNameValidation = true;
            dashboardVM.IgnoreBuzzlingNameValidation = false;

            ModelState.Remove("HiveName");

            Buzzling buzzling = await _buzzlingService.GetById(dashboardVM.BuzzlingId!.Value);

            if (ModelState.IsValid)
            {
                buzzling.Name = dashboardVM.BuzzlingName;
                buzzling.Role = await _unitOfWork.BuzzlingRoleRepository.Get((r) => r.Id == Int32.Parse(dashboardVM.BuzzlingRole!));

                await _buzzlingService.Update(buzzling);

                dashboardVM.BuzzlingName = String.Empty;
                dashboardVM.BuzzlingRole = "0";

                dashboardVM.IgnoreBuzzlingNameValidation = true;
            }
            else
            {
                dashboardVM.IsUpdateAttempt = true;
            }

            return RedirectToAction("Index", "Dashboard", dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBuzzling(DashboardViewModel dashboardVM, int id)
        {
            dashboardVM.IgnoreHiveNameValidation = true;
            dashboardVM.IgnoreBuzzlingNameValidation = true;

            Buzzling buzzling = await _buzzlingService.GetById(id);

            await _buzzlingService.Delete(buzzling);

            return RedirectToAction("Index", "Dashboard", dashboardVM);
        }

        public async Task UpdateSimulationState()
        {

        }

        public async Task<IActionResult> GetEventLog(int lastLogIndex = 0)
        {
            User user = await _userService.GetUser(User);

            if (user.HiveId.HasValue)
            {
                Hive hive = await _hiveService.Get(h => h.Id == user.HiveId);

                if (hive.EventLog is null)
                {
                    hive.EventLog = new List<string>();

                    hive.EventLog?.Add("🍯 " + hive.Name + " 🍯 is born!");

                    await _hiveService.Update(hive);
                }

                List<string>? newLogs = hive.EventLog?.Skip(lastLogIndex).ToList();

                int updatedLogIndex = hive.EventLog!.Count;

                return Json(new { log = newLogs, lastLogIndex = updatedLogIndex });
            }

            return Json(new { log = new List<string>() });
        }

        public async Task<IActionResult> FilterBuzzlings(string query)
        {
            List<Buzzling> buzzlings = new List<Buzzling>();

            User user = await _userService.GetUser(User);

            if (user.HiveId.HasValue)
            {
                Hive hive = await _hiveService.GetWithBuzzlingsAndRoles(h => h.Id == user.HiveId);

                if (hive.Buzzlings is not null)
                {
                    buzzlings = hive.Buzzlings.Where(b =>
                        string.IsNullOrEmpty(query) || //If the query is empty, return full list
                        string.IsNullOrWhiteSpace(query) ||
                        b.Name!.Contains(query, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            return PartialView("_BuzzlingsTablePartial", buzzlings);
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
