using System.Text.Json.Serialization;

namespace Szakdoga.Models
{
    public class TuyaEntity : Entity
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
    }
}
