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

            //When a Hive is deleted, is should first null the 
            //Foreign Key reference in the User. Otherwise it'll throw an error.
            //This is needed when deleting async only, otherwise EFC takes care of it itself...
            modelBuilder.Entity<User>()
                .HasOne(u => u.Hive) //Can't use .HasOne<Hive>() as it'll create a new field in the table
                .WithMany()
                .HasForeignKey(u => u.HiveId)
                .OnDelete(DeleteBehavior.SetNull);

            //When a Hive is deleted, we should delete all of its Buzllings too
            modelBuilder.Entity<Buzzling>()
                .HasOne(b => b.Hive)
                .WithMany(h => h.Buzzlings)
                .HasForeignKey(b => b.HiveId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
