using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Data.Models
{
    public class Hive
    {
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        public int? Age { get; set; }
        public ICollection<Buzzling>? Buzzlings { get; set; }
        public ICollection<string>? EventLog { get; set; }
    }
}
