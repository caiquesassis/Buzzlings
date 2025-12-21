using Microsoft.AspNetCore.Identity;

namespace Buzzlings.BusinessLogic.Services.User
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterNewUserAsync(string username, string password);
        Task<(IdentityResult, Data.Models.User?)> ChangeUserNameAsync(string? userId, string newUsername);
        Task<IEnumerable<Data.Models.User>> GetAllUsersAsync();
        Task<Data.Models.User?> GetUserByIdAsync(string? userId, bool includeHive = false, bool includeBuzzlings = false, bool includeBuzzlingsRoles = false);
        Task<Data.Models.User?> GetUserByUserNameAsync(string username);
        Task<string> GetUserIdAsync(Data.Models.User user);
        Task<bool> CheckUserPasswordAsync(Data.Models.User user, string password);
        Task<IdentityResult> UpdateUserAsync(Data.Models.User user);
        Task<(IdentityResult, Data.Models.User?)> UpdateUserPasswordAsync(string? userId, string currentPassword, string newPassword);
        Task<IdentityResult> UpdateUserSecurityStampAsync(string? userId);
        Task<IdentityResult> DeleteUserAsync(string? userId);
    }
}
