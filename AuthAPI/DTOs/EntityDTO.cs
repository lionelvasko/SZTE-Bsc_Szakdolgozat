using System.Text.Json.Serialization;

namespace AuthAPI.DTOs
{
    /// <summary>
    /// Represents the base class for entity data transfer objects (DTOs).
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(TuyaEntityDTO), "tuya")]
    [JsonDerivedType(typeof(SomfyEntityDTO), "somfy")]
    public abstract class EntityDTO
    {
        /// <summary>
        /// Gets or sets the URL associated with the entity.
        /// </summary>
        public required string URL { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the platform of the entity.
        /// </summary>
        public required string Platform { get; set; }

        /// <summary>
        /// Gets or sets the icon representing the entity.
        /// </summary>
        public required string Icon { get; set; }
    }
}
