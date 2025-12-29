using Buzzlings.Data.Models;
using Buzzlings.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Web.Models
{
    public class DashboardViewModel
    {
        public User? User { get; set; }
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Hive name must be 3 - 20 characters long.")]
        [Display(Name = "Hive name")]
        [Required(ErrorMessage = "Hive name is required.")]
        [NotWhitespace(ErrorMessage = "Hive name cannot contain only whitespace.")]
        public string? HiveName { get; set; }
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Buzzling name must be 3 - 20 characters long.")]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Buzzling name is required.")]
        [NotWhitespace(ErrorMessage = "Buzzling name cannot contain only whitespace.")]
        public string? BuzzlingName { get; set; }
        public string? BuzzlingRole { get; set; }
        public int? BuzzlingId { get; set; }
    }
}
