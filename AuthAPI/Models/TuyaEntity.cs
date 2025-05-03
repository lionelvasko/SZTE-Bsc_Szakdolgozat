namespace AuthAPI.Models
{
    /// <summary>
    /// Represents a Tuya entity with access and refresh tokens and region information.
    /// </summary>
    public class TuyaEntity : Entity
    {
        /// <summary>
        /// Gets or sets the access token for the Tuya entity.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token for the Tuya entity.
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the region for the Tuya entity.
        /// </summary>
        public required string Region { get; set; }
    }
}
