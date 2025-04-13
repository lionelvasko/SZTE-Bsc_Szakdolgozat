using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    public class Device
    {
        [JsonPropertyName("data")]
        public DeviceData Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("dev_type")]
        public string DevType { get; set; }

        [JsonPropertyName("ha_type")]
        public string HaType { get; set; }
    }
}
