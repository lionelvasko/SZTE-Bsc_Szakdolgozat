using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    public class Header
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("payloadVersion")]
        public int PayloadVersion { get; set; }
    }
}
