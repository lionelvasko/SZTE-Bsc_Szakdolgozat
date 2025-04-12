using System.Text.Json.Serialization;

namespace Szakdoga.Models
{
    public class SomfyEntity : Entity
    {
        [JsonPropertyName("baseUrl")]
        public string BaseUrl { get; set; }
        [JsonPropertyName("gatewayPin")]
        public string GatewayPin { get; set; }
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
