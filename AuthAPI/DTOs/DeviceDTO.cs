namespace AuthAPI.DTOs
{
    /// <summary>
    /// Represents a device with associated entities and metadata.
    /// </summary>
    public class DeviceDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the device.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation time of the device.
        /// </summary>
        public string CreationTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the platform of the device.
        /// </summary>
        public string Platform { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of associated Somfy entities.
        /// </summary>
        public ICollection<SomfyEntityDTO> SomfyEntities { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of associated Tuya entities.
        /// </summary>
        public ICollection<TuyaEntityDTO> TuyaEntities { get; set; } = [];
    }
}
