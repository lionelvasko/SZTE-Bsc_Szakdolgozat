using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    /// <summary>  
    /// Represents a device entity with details such as name, platform, and associated user.  
    /// </summary>  
    public class Device
    {
        /// <summary>  
        /// Gets or sets the unique identifier for the device.  
        /// </summary>  
        [Key]
        public Guid Id { get; set; }

        /// <summary>  
        /// Gets or sets the name of the device.  
        /// </summary>  
        public required string Name { get; set; } = string.Empty;

        /// <summary>  
        /// Gets or sets the creation time of the device.  
        /// </summary>  
        public required string CreationTime { get; set; } = string.Empty;

        /// <summary>  
        /// Gets or sets the platform of the device (e.g., iOS, Android).  
        /// </summary>  
        public required string Platform { get; set; } = string.Empty;

        /// <summary>  
        /// Gets or sets the user ID associated with the device.  
        /// </summary>  
        public required string UserId { get; set; }
    }

}
