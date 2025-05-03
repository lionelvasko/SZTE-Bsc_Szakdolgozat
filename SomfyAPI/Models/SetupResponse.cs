using System.Text.Json.Serialization;

namespace SomfyAPI.Models
{
    public class SetupResponse
    {
        [JsonPropertyName("gateways")]
        public List<Device>? Devices { get; set; }
        [JsonPropertyName("devices")]
        public List<Entity>? Entities { get; set; }
    }
}
