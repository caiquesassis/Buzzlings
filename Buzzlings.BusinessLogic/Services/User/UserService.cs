using Buzzlings.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Buzzlings.BusinessLogic.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Data.Models.User> _userManager;

        public UserService(UserManager<Data.Models.User> userManager) 
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(Data.Models.User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IEnumerable<Data.Models.User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<Data.Models.User?> GetUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<Data.Models.User?> GetByIdAsync(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString()!);
        }

        public async Task<Data.Models.User?> GetByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<string> GetIdAsync(Data.Models.User user)
        {
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(Data.Models.User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(Data.Models.User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdatePasswordAsync(Data.Models.User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateSecurityStampAsync(Data.Models.User user)
        {
            return await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(Data.Models.User user)
        {
            return await _userManager.DeleteAsync(user);
        }
    }
}
