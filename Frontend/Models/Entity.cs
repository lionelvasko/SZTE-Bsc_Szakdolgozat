using System.Text.Json.Serialization;

namespace Szakdoga.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(TuyaEntity), "tuya")]
    [JsonDerivedType(typeof(SomfyEntity), "somfy")]
    public class Entity
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("platform")]
        public string Platform { get; set; }
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}
