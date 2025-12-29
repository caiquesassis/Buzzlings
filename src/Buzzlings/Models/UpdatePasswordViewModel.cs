using Buzzlings.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Web.Models
{
    public class UpdatePasswordViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [NotWhitespace(ErrorMessage = "Password cannot contain only whitespace.")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [NotWhitespace(ErrorMessage = "Password cannot contain only whitespace.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }
}
