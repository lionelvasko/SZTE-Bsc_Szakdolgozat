namespace AuthAPI.Models
{
    /// <summary>  
    /// Represents a Somfy entity with specific properties for integration.  
    /// </summary>  
    public class SomfyEntity : Entity
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