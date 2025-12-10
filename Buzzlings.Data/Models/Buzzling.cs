using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buzzlings.Data.Models
{
    public class Buzzling
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public int? RoleId;
        [ForeignKey("RoleId")]
        public BuzzlingRole? Role { get; set; }
        [Range(0, 100)]
        public int? Mood { get; set; } = 100;
        public int? HiveId { get; set; }

        [ForeignKey("HiveId")]
        public Hive? Hive { get; set; }
    }
}
