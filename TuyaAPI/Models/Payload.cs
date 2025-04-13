using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    public class Payload
    {
        [JsonPropertyName("devices")]
        public List<Device> Devices { get; set; }

        [JsonPropertyName("scenes")]
        public List<object> Scenes { get; set; } // Adjust if scenes have a structure
    }
}
