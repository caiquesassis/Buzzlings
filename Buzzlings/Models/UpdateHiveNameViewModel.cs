using Buzzlings.Data.Models;
using Buzzlings.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Web.Models
{
    public class UpdateHiveNameViewModel
    {
        public User? User { get; set; }
        [Required(ErrorMessage = "Hive name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Hive name must be 3 - 20 characters long.")]
        [NotWhitespace(ErrorMessage = "Hive name cannot contain only whitespace.")]
        [Display(Name = "Hive name")]
        public string? HiveName { get; set; }
    }
}
