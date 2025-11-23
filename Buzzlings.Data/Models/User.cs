using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buzzlings.Data.Models
{
    public class User : IdentityUser
    {
        public int? HiveId { get; set; }
        [ForeignKey("HiveId")]
        public Hive? Hive { get; set; }
    }
}
