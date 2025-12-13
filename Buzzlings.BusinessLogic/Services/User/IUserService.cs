using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Buzzlings.BusinessLogic.Services.User
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(Data.Models.User user, string password);
        Task<IEnumerable<Data.Models.User>> GetAllAsync();
        Task<Data.Models.User?> GetUserAsync(ClaimsPrincipal principal);
        Task<Data.Models.User?> GetByIdAsync(int id);
        Task<Data.Models.User?> GetByUsernameAsync(string username);
        Task<string> GetIdAsync(Data.Models.User user);
        Task<bool> CheckPasswordAsync(Data.Models.User user, string password);
        Task<IdentityResult> UpdateAsync(Data.Models.User user);
        Task<IdentityResult> UpdatePasswordAsync(Data.Models.User user, string currentPassword, string newPassword);
        Task<IdentityResult> UpdateSecurityStampAsync(Data.Models.User user);
        Task<IdentityResult> DeleteAsync(Data.Models.User user);
    }
}
