using System.Text.Json.Serialization;

namespace Szakdoga.Models
{
    public class Device
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("creationTime")]
        public string CreationTime { get; set; } = string.Empty;
        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;
        public ICollection<SomfyEntity> SomfyEntities { get; set; } = [];
        public ICollection<TuyaEntity> TuyaEntities { get; set; } = [];
    }
}
