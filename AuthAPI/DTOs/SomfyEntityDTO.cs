namespace AuthAPI.DTOs
{
    /// <summary>
    /// Represents a Somfy entity with specific properties such as BaseUrl, CloudUsername, and CloudPasswordHashed.
    /// Inherits from the base EntityDTO class.
    /// </summary>
    public class SomfyEntityDTO : EntityDTO
    {
        /// <summary>
        /// Gets or sets the base URL for the Somfy entity.
        /// </summary>
        public required string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the cloud username for the Somfy entity.
        /// </summary>
        public required string CloudUsername { get; set; }

        /// <summary>
        /// Gets or sets the hashed cloud password for the Somfy entity.
        /// </summary>
        public required string CloudPasswordHashed { get; set; }
    }
}
