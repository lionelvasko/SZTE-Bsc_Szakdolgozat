using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    /// <summary>
    /// Represents the application's database context, which includes identity and custom entities.
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;
        private readonly EncryptService _encryptService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the database context.</param>
        /// <param name="configuration">The application configuration.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var encryptSecret = _configuration["Encrypt:Secret"]
                                ?? throw new InvalidOperationException("Encryption secret is not configured.");
            _encryptService = new EncryptService(encryptSecret);
            Database.Migrate();
        }

        /// <summary>
        /// Gets or sets the collection of devices in the database.
        /// </summary>
        public DbSet<Device> Devices { get; set; }

        /// <summary>
        /// Gets or sets the collection of Tuya entities in the database.
        /// </summary>
        public DbSet<TuyaEntity> TuyaEntities { get; set; }

        /// <summary>
        /// Gets or sets the collection of Somfy entities in the database.
        /// </summary>
        public DbSet<SomfyEntity> SomfyEntities { get; set; }

        /// <summary>
        /// Configures the model relationships and properties for the database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the database schema.</param>
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
