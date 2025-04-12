using System.Text.Json.Serialization;

namespace AuthAPI.DTOs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(TuyaEntityDTO), "tuya")]
    [JsonDerivedType(typeof(SomfyEntityDTO), "somfy")]
    public abstract class EntityDTO
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Icon { get; set; }
    }
}
