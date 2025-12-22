using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Data.Models
{
    public class Hive
    {
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        public int? Age { get; set; } = 0;
        [Range(0, 100)]
        public int? Happiness { get; set; } = 100;
        public ICollection<Buzzling>? Buzzlings { get; set; }
        public List<string>? EventLog { get; set; }
    }
}
