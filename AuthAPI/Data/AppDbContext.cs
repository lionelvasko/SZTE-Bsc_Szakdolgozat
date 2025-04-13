  using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;
        private readonly EncryptService _encryptService;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            _encryptService = new EncryptService(_configuration["Encrypt:Secret"]);
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<TuyaEntity> TuyaEntities { get; set; }
        public DbSet<SomfyEntity> SomfyEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Device 
            modelBuilder.Entity<Device>()
                .HasOne<User>()
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Device → TuyaEntity
            modelBuilder.Entity<TuyaEntity>()
                .HasOne<Device>()
                .WithMany()
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SomfyEntity>()
                .HasOne<Device>()
                .WithMany()
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SomfyEntity>()
                .Property(e => e.CloudPasswordHashed)
                .HasConversion(new EncryptedStringConverter(_encryptService));
        }
    }

}
