using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Buzzlings.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddIdentityErrors(this ModelStateDictionary modelState, IdentityResult result, string keyName)
        {
            foreach (IdentityError error in result.Errors)
            {
                modelState.AddModelError(keyName, error.Description);
            }
        }

        public static void AddIdentityErrors(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors, string keyName)
        {
            foreach (IdentityError error in errors)
            {
                modelState.AddModelError(keyName, error.Description);
            }
        }
    }
}
