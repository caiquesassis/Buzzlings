using Buzzlings.BusinessLogic.Services.Hive;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Buzzlings.BusinessLogic.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Data.Models.User> _userManager;
        private readonly IHiveService _hiveService;

        public UserService(UserManager<Data.Models.User> userManager, IHiveService hiveService)
        {
            _userManager = userManager;
            _hiveService = hiveService;
        }

        public async Task<IdentityResult> RegisterNewUserAsync(string userName, string password)
        {
            Data.Models.User user = new() { UserName = userName };

            // CreateAsync will return a failed result if the username is taken
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<(IdentityResult, Data.Models.User?)> ChangeUserNameAsync(string? userId, string newUserName)
        {
            Data.Models.User? user = await GetUserByIdAsync(userId);

            if (user is null)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "User not found." }), null);
            }

            // SetUserNameAsync validates the name and checks for duplicates automatically
            IdentityResult result = await _userManager.SetUserNameAsync(user, newUserName);

            if (result.Succeeded is false)
            {
                return (result, user);
            }

            return (await _userManager.UpdateAsync(user), user);
        }

        public async Task<IEnumerable<Data.Models.User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<Data.Models.User?> GetUserByIdAsync(string? userId, bool includeHive = false, bool includeBuzzlings = false, bool includeBuzzlingsRoles = false)
        {
            if (userId is null)
            {
                return null;
            }

            Data.Models.User? user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return null;
            }

            if (includeHive is false || user.HiveId.HasValue is false)
            {
                return user;
            }

            if (includeBuzzlings is false)
            {
                user.Hive = await _hiveService.GetHiveAsync(h => h.Id == user.HiveId);
                return user;
            }

            if (includeBuzzlingsRoles is false)
            {
                user.Hive = await _hiveService.GetHiveAsync(h => h.Id == user.HiveId, "Buzzlings");
                return user;
            }

            user.Hive = await _hiveService.GetHiveWithBuzzlingsAndRolesAsync(h => h.Id == user.HiveId);

            return user;
        }

        public async Task<Data.Models.User?> GetUserByUserNameAsync(string? userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> GetUserIdAsync(Data.Models.User user)
        {
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<bool> CheckUserPasswordAsync(Data.Models.User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(Data.Models.User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<(IdentityResult, Data.Models.User?)> UpdateUserPasswordAsync(string? userId, string currentPassword, string newPassword)
        {
            Data.Models.User? user = await GetUserByIdAsync(userId);

            if (user is null)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "User not found." }), null);
            }

            return (await _userManager.ChangePasswordAsync(user, currentPassword, newPassword), user);
        }

        public async Task<IdentityResult> UpdateUserSecurityStampAsync(string? userId)
        {
            Data.Models.User? user = await GetUserByIdAsync(userId);

            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            return await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string? userId)
        {
            Data.Models.User? user = await GetUserByIdAsync(userId);

            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            if (user.HiveId is not null)
            {
                Data.Models.Hive? hive = await _hiveService.GetHiveAsync(h => h.Id == user.HiveId, "Buzzlings");

                if (hive is not null)
                {
                    await _hiveService.DeleteHiveAsync(hive);
                }
            }

            return await _userManager.DeleteAsync(user);
        }
    }
}
