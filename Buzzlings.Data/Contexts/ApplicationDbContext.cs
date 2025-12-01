using Microsoft.EntityFrameworkCore;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Buzzlings.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<Hive> Hives { get; set; }
        public DbSet<Buzzling> Buzzlings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuzzlingRole>().HasData(
                new { Id = 1, Name = "Worker" },
                new { Id = 2, Name = "Guard" },
                new { Id = 3, Name = "Forager"},
                new { Id = 4, Name = "Drone"},
                new { Id = 5, Name = "Nurse"},
                new { Id = 6, Name = "Attendant"}
            );

            modelBuilder.Entity<Buzzling>()
                .HasOne(b => b.Hive)
                .WithMany(h => h.Buzzlings)
                .HasForeignKey(b => b.HiveId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
