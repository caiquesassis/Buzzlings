using Buzzlings.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Web.Models
{
    public class UpdateUsernameViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be 3 - 20 characters long.")]
        [NotWhitespace(ErrorMessage = "Username cannot contain only whitespace.")]
        public string? Username { get; set; }
    }
}
