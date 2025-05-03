using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    /// <summary>
    /// Represents a base entity with common properties for all derived models.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier associated with the entity.
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the device identifier associated with the entity.
        /// </summary>
        public Guid DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the URL associated with the entity.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the platform associated with the entity.
        /// </summary>
        public string Platform { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the icon associated with the entity.
        /// </summary>
        public string Icon { get; set; } = string.Empty;
    }

}
