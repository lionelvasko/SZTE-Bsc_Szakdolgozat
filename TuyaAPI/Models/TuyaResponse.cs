using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    public class TuyaResponse
    {
        [JsonPropertyName("payload")]
        public Payload? Payload { get; set; }

        [JsonPropertyName("header")]
        public Header? Header { get; set; }
    }
}
