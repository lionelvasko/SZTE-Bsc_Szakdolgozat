using Microsoft.EntityFrameworkCore;
using AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SomfyDevice> SomfyDevices { get; set; }
        public DbSet<TuyaDevice> TuyaDevices { get; set; }

        public DbSet<SomfyEntity> SomfyEntities { get; set; }
        public DbSet<TuyaEntity> TuyaEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasMany(u => u.SomfyDevices)
                .WithOne()
                .HasForeignKey("UserId");

            builder.Entity<User>()
                .HasMany(u => u.TuyaDevices)
                .WithOne()
                .HasForeignKey("UserId");
        }

    }
}
