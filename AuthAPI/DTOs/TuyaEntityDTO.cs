namespace AuthAPI.DTOs
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for Tuya entities.
    /// </summary>
    public class TuyaEntityDTO : EntityDTO
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
        /// Gets or sets the region associated with the Tuya entity.
        /// </summary>
        public required string Region { get; set; }
    }
}
