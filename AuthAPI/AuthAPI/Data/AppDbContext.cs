using Microsoft.EntityFrameworkCore;
using AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Entity> Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Device
            modelBuilder.Entity<Device>()
                .HasOne<User>()
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Device → Entity
            modelBuilder.Entity<Entity>()
                .HasOne<Device>()
                .WithMany(d => d.Entities)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
