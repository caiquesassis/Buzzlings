using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Buzzlings.BusinessLogic.Services.User
{
    public interface IUserService
    {
        Task<IdentityResult> Create(Data.Models.User user, string password);
        Task<IEnumerable<Data.Models.User>> GetAll();
        Task<Data.Models.User> GetUser(ClaimsPrincipal principal);
        Task<Data.Models.User> GetById(int id);
        Task<Data.Models.User> GetByUsername(string username);
        Task<string> GetId(Data.Models.User user);
        Task<bool> CheckPassword(Data.Models.User user, string password);
        Task<IdentityResult> Update(Data.Models.User user);
        Task<IdentityResult> UpdatePassword(Data.Models.User user, string currentPassword, string newPassword);
        Task<IdentityResult> UpdateSecurityStamp(Data.Models.User user);
        Task<IdentityResult> Delete(Data.Models.User user);
    }
}
