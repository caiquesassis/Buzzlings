using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buzzlings.Data.Models
{
    public class TopHive
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [Required]
        public string? HiveName { get; set; }
        [Required]
        public int HiveAge { get; set; }
    }
}
