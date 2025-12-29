using System.ComponentModel.DataAnnotations;
using Buzzlings.Web.Attributes;

namespace Buzzlings.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be 3 - 20 characters long.")]
        [NotWhitespace(ErrorMessage = "Username cannot contain only whitespace.")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [NotWhitespace(ErrorMessage = "Password cannot contain only whitespace.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
