using AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
        public DbSet<TuyaEntity> TuyaEntities { get; set; }
        public DbSet<SomfyEntity> SomfyEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Device kapcsolat
            modelBuilder.Entity<Device>()
                .HasOne<User>()
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Device → TuyaEntity kapcsolat
            modelBuilder.Entity<TuyaEntity>()
                .HasOne<Device>()
                .WithMany(d => d.TuyaEntities) // Külön TuyaEntities kollekció
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Device → SomfyEntity kapcsolat
            modelBuilder.Entity<SomfyEntity>()
                .HasOne<Device>()
                .WithMany(d => d.SomfyEntities) // Külön SomfyEntities kollekció
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
