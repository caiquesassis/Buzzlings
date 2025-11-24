using Buzzlings.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Web.Models
{
    public class DashboardViewModel
    {
        public User? User { get; set; }
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Hive name must be 3 - 20 characters long.")]
        [Display(Name = "Hive name")]
        [Required(ErrorMessage = "Hive name is required.")]
        public string? HiveName { get; set; }
        public bool IgnoreHiveNameValidation { get; set; } = true;
    }
}
