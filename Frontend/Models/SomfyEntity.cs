using System.Text.Json.Serialization;

namespace Szakdoga.Models
{
    public class SomfyEntity : Entity
    {
        [JsonPropertyName("baseUrl")]
        public string BaseUrl { get; set; }
        [JsonPropertyName("gatewayPin")]
        public string GatewayPin { get; set; }
        [JsonPropertyName("cloudUsername")]
        public string CloudUsername { get; set; }
        [JsonPropertyName("cloudPasswordHashed")]
        public string CloudPasswordHashed { get; set; }
    }
}
